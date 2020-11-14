using Baer.Application.SystemManage;
using Baer.Code;
using Baer.Data.Extensions;
using Baer.Domain.Entity.SystemManage;
using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Baer.Web.Areas.SystemManage.Controllers
{
    public class CodeGeneratorController : ControllerBase
    {
        private TableApp tableApp = new TableApp();
        private TableFieldApp app = new TableFieldApp();

        public CodeGeneratorController()
        {
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            ViewBag.Creator = LoginInfo.UserName;
            string filePath = System.Web.HttpContext.Current.Request.MapPath("~");
            filePath = filePath.Trim('\\');
            filePath = filePath.Substring(0, filePath.LastIndexOf('\\'));
            ViewBag.DomainPath = filePath + "\\Baer.Domain";
            ViewBag.RepositoryPath = filePath + "\\Baer.Repository";
            ViewBag.MapPath = filePath + "\\Baer.Mapping";
            ViewBag.ApplicationPath = filePath + "\\Baer.Application";
            ViewBag.WebPath = filePath + "\\Baer.Web";
        }

        #region ==========列表==========
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string tableName)
        {
            string ret = "[{";
            string tableId = DbHelper.ExecuteToString("select Id from Sys_TableInfo where TableName='" + tableName + "'");
            var fieldList = app.GetList(tableId);
            foreach (TableFieldEntity field in fieldList)
            {
                ret += "'" + field.FieldName + "':'',";
            }
            ret = ret.Trim(',') + "}]";
            return Content(ret);
        }

        public ActionResult GetTableFieldTree(string tableName)
        {
            string tableId = DbHelper.ExecuteToString("select Id from Sys_TableInfo where TableName='" + tableName + "'");
            var fieldList = app.GetList(tableId);
            var treeList = new List<TreeViewModel>();

            //Tabel
            TreeViewModel tree = new TreeViewModel();
            tree.id = tableId;
            tree.text = tableName;
            tree.value = tableName;
            tree.parentId = "0";
            tree.isexpand = true;
            tree.complete = true;
            tree.showcheck = false;
            tree.checkstate = 0;
            tree.hasChildren = true;
            tree.img = "";
            treeList.Add(tree);

            //TableField
            foreach (TableFieldEntity field in fieldList)
            {
                TreeViewModel node = new TreeViewModel();
                node.id = field.FieldName;
                node.text = field.FieldChineseName;
                node.value = field.FieldName;
                node.parentId = tableId;
                node.isexpand = false;
                node.complete = true;
                node.showcheck = true;
                node.checkstate = 0;
                node.hasChildren = false;
                tree.img = "";
                treeList.Add(node);
            }
            return Content(treeList.TreeViewJson());
        }

        public ActionResult GetModelDirectory()
        {
            string filePath = System.Web.HttpContext.Current.Request.MapPath("~");
            filePath += "Areas\\";
            DirectoryInfo dir = new DirectoryInfo(filePath);
            DirectoryInfo[] dii = dir.GetDirectories();
            List<object> obj = new List<object>();
            foreach (DirectoryInfo item in dii)
            {
                obj.Add(new { id = item.Name, text = item.Name });
            }
            return Content(obj.ToJson());
        }

        public ActionResult ListIndex()
        {
            return View();
        }
        #endregion

        #region ==========提交==========
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(TableEntity entity)
        {
            tableApp.CodeGeneratorUpdate(entity);
            return Success("预览代码生成成功！");
        }
        #endregion

        #region ==========预览==========
        public ActionResult Preview()
        {
            return View();
        }

        public ActionResult PreviewCode(string tableId)
        {
            TableEntity entity = tableApp.GetForm(tableId);
            object obj = new
            {
                entityCode = HttpUtility.HtmlEncode(GetEntityCode(tableId, entity)),
                iRepositoryCode = HttpUtility.HtmlEncode(GetIRepositoryCode(tableId, entity)),
                repositoryCode = HttpUtility.HtmlEncode(GetRepositoryCode(tableId, entity)),
                mappingCode = HttpUtility.HtmlEncode(GetMappingCode(tableId, entity)),
                applicationCode = HttpUtility.HtmlEncode(GetApplicationCode(tableId, entity)),
                controllerCode = HttpUtility.HtmlEncode(GetControllorCode(tableId, entity)),
                registerCode = HttpUtility.HtmlEncode(GetRegisterCode(tableId, entity)),
                listCode = HttpUtility.HtmlEncode(GetListCode(tableId, entity)),
                formCode = HttpUtility.HtmlEncode(GetFormCode(tableId, entity)),
                detailsCode = HttpUtility.HtmlEncode(GetDetailsCode(tableId, entity))
            };
            return Content(obj.ToJson());
        }
        #endregion

        #region ==========生成==========
        public ActionResult CreateCode(string tableId)
        {
            TableEntity entity = tableApp.GetForm(tableId);

            #region 写入文件
            //实体类
            string entityFileName = "\\03 Entity\\" + entity.OutputModel + "\\" + entity.ClassPrefix + "Entity.cs";
            CreateFile(ViewBag.DomainPath + entityFileName, GetEntityCode(tableId, entity));
            IncludingCsproj(ViewBag.DomainPath + "\\Baer.Domain.csproj", entityFileName);

            //接口类
            string iRepositoryFileName = "\\04 IRepository\\" + entity.OutputModel + "\\I" + entity.ClassPrefix + "Repository.cs";
            CreateFile(ViewBag.DomainPath + iRepositoryFileName, GetIRepositoryCode(tableId, entity));
            IncludingCsproj(ViewBag.DomainPath + "\\Baer.Domain.csproj", iRepositoryFileName);

            //仓库类
            string repositoryFileName = "\\" + entity.OutputModel + "\\" + entity.ClassPrefix + "Repository.cs";
            CreateFile(ViewBag.RepositoryPath + repositoryFileName, GetRepositoryCode(tableId, entity));
            IncludingCsproj(ViewBag.RepositoryPath + "\\Baer.Repository.csproj", repositoryFileName);

            //映射类
            string mapFileName = "\\" + entity.OutputModel + "\\" + entity.ClassPrefix + "Map.cs";
            CreateFile(ViewBag.MapPath + mapFileName, GetMappingCode(tableId, entity));
            IncludingCsproj(ViewBag.MapPath + "\\Baer.Mapping.csproj", mapFileName);

            //业务类
            string appFileName = "\\" + entity.OutputModel + "\\" + entity.ClassPrefix + "App.cs";
            CreateFile(ViewBag.ApplicationPath + appFileName, GetApplicationCode(tableId, entity));
            IncludingCsproj(ViewBag.ApplicationPath + "\\Baer.Application.csproj", appFileName);

            //控制器
            string controllerFileName = "\\Areas\\" + entity.OutputModel + "\\Controllers\\" + entity.ClassPrefix + "Controller.cs";
            CreateFile(ViewBag.WebPath + controllerFileName, GetControllorCode(tableId, entity));
            IncludingCsproj(ViewBag.WebPath + "\\Baer.Web.csproj", controllerFileName);

            //注册表
            string registerFileName = "\\Areas\\" + entity.OutputModel + "\\" + entity.OutputModel + "AreaRegistration.cs";
            if (Directory.Exists(Path.GetDirectoryName(ViewBag.WebPath + registerFileName)))
            {
                CreateFile(ViewBag.WebPath + registerFileName, GetRegisterCode(tableId, entity));
                IncludingCsproj(ViewBag.WebPath + "\\Baer.Web.csproj", registerFileName);
            }

            //列表页
            string listFileName = "\\Areas\\" + entity.OutputModel + "\\Views\\" + entity.ClassPrefix + "\\" + entity.ClassPrefix + "Index.cshtml";
            CreateFile(ViewBag.WebPath + listFileName, GetListCode(tableId, entity));
            IncludingCsproj(ViewBag.WebPath + "\\Baer.Web.csproj", listFileName, "Content");

            //表单页
            string formFileName = "\\Areas\\" + entity.OutputModel + "\\Views\\" + entity.ClassPrefix + "\\" + entity.ClassPrefix + "Form.cshtml";
            CreateFile(ViewBag.WebPath + formFileName, GetFormCode(tableId, entity));
            IncludingCsproj(ViewBag.WebPath + "\\Baer.Web.csproj", formFileName, "Content");

            //明细页
            string detailsFileName = "\\Areas\\" + entity.OutputModel + "\\Views\\" + entity.ClassPrefix + "\\" + entity.ClassPrefix + "Details.cshtml";
            CreateFile(ViewBag.WebPath + detailsFileName, GetDetailsCode(tableId, entity));
            IncludingCsproj(ViewBag.WebPath + "\\Baer.Web.csproj", detailsFileName, "Content");
            #endregion

            #region 生成菜单
            string parentModuleId = DbHelper.ExecuteToString("select Id from Sys_Module where F_EnCode='CodeGenerator'");//主菜单
            if(DbHelper.ExecuteToDataView("select 1 from Sys_Module where F_UrlAddress='/CodeGenerator/" + entity.TableName + "/" + entity.TableName + "Index'").Count == 0)
            {
                //写入菜单
                ModuleEntity moduleEntity = new ModuleEntity()
                {
                    F_ParentId = parentModuleId,
                    F_FullName = entity.TableChineseName,
                    F_UrlAddress = "/CodeGenerator/" + entity.ClassPrefix + "/" + entity.ClassPrefix + "Index",
                    F_Target = "iframe",
                    F_IsMenu = true,
                    F_IsExpand = false,
                    F_IsPublic = false,
                    F_AllowEdit = true,
                    F_AllowDelete = true,
                    F_SortCode = DbHelper.ExecuteToDataView("select 1 from Sys_Module where F_ParentId='" + parentModuleId + "'").Count + 1,
                    F_EnabledMark = true
                };
                ModuleApp moduleApp = new ModuleApp();
                string moduleId = moduleApp.SubmitForm(moduleEntity, "");

                ModuleButtonEntity moduleButtonEntity = new ModuleButtonEntity();
                moduleButtonEntity = new ModuleButtonEntity()
                {
                    F_ModuleId = moduleId,
                    F_ParentId = "0",
                    F_Layers = 1,
                    F_Location = 2,
                    F_Split = false,
                    F_IsPublic = false,
                    F_AllowEdit = true,
                    F_AllowDelete = true,
                    F_EnabledMark = true
                };

                //写入按钮
                for (int i = 0; i < 4; i++)
                {
                    moduleButtonEntity.F_SortCode = i;
                    switch (i)
                    {
                        case 0://删除
                            moduleButtonEntity.F_EnCode = "btnDelete";
                            moduleButtonEntity.F_FullName = "删除";
                            moduleButtonEntity.F_JsEvent = "deletes()";
                            moduleButtonEntity.F_UrlAddress = "/" + entity.OutputModel + "/" + entity.ClassPrefix + "Delete";
                            break;
                        case 1://新建
                            moduleButtonEntity.F_EnCode = "btnAdd";
                            moduleButtonEntity.F_FullName = "新建";
                            moduleButtonEntity.F_JsEvent = "operate()";
                            moduleButtonEntity.F_UrlAddress = "/" + entity.OutputModel + "/" + entity.ClassPrefix + "Form";
                            break;
                        case 2://修改
                            moduleButtonEntity.F_EnCode = "btnEdit";
                            moduleButtonEntity.F_FullName = "修改";
                            moduleButtonEntity.F_JsEvent = "operate()";
                            moduleButtonEntity.F_UrlAddress = "/" + entity.OutputModel + "/" + entity.ClassPrefix + "Form";
                            break;
                        case 3://查看
                            moduleButtonEntity.F_EnCode = "btnDetails";
                            moduleButtonEntity.F_FullName = "修改";
                            moduleButtonEntity.F_JsEvent = "details()";
                            moduleButtonEntity.F_UrlAddress = "/" + entity.OutputModel + "/" + entity.ClassPrefix + "Details";
                            break;
                        default:
                            break;
                    }

                    ModuleButtonApp moduleButtonApp = new ModuleButtonApp();
                    moduleButtonApp.SubmitForm(moduleButtonEntity, "");
                }
            }
            #endregion

            return Content("");
        }
        #endregion

        #region ==========代码==========
        private string GetEntityCode(string tableId, TableEntity tableEntity)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine("using Baer.Domain.Infrastructure;");
            code.AppendLine("using System;");
            code.AppendLine();
            code.AppendLine("namespace Baer.Domain.Entity." + tableEntity.OutputModel);
            code.AppendLine("{");
            GetAuthor(code, tableEntity);
            code.AppendLine("    public class " + tableEntity.ClassPrefix + "Entity : BaseEntity");
            code.AppendLine("    {");
            //业务属性
            List<TableFieldEntity> tableFieldEntitys = app.GetList(tableId);
            foreach (TableFieldEntity field in tableFieldEntitys)
            {
                string fieldType = field.FieldType;
                switch (field.FieldType)
                {
                    case "binary":
                    case "image":
                        fieldType = "Byte[]";
                        break;
                    case "bit":
                        fieldType = "Boolean";
                        break;
                    case "char":
                    case "nchar":
                    case "varchar":
                    case "nvarchar":
                    case "text":
                        fieldType = "String";
                        break;
                    case "int":
                        fieldType = "Int32";
                        break;
                    case "bigint":
                        fieldType = "Int64";
                        break;
                    case "float":
                        fieldType = "Double";
                        break;
                    case "money":
                    case "numeric":
                    case "decimal":
                        fieldType = "Decimal";
                        break;
                    case "date":
                    case "datetime":
                    case "datetime2":
                        fieldType = "DateTime?";
                        break;
                    case "datetimeoffset":
                        fieldType = "DateTimeOffset";
                        break;
                    default:
                        break;
                }
                GetFieldDescription(code, field.Description);
                code.AppendLine("        public " + fieldType + " " + field.FieldName + " { get; set; }");
                code.AppendLine();
            }
            code.AppendLine("    }");
            code.AppendLine("}");
            return code.ToString();
        }

        private string GetIRepositoryCode(string f_Id, TableEntity tableEntity)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine("using Baer.Data;");
            code.AppendLine("using Baer.Domain.Entity." + tableEntity.OutputModel + ";");
            code.AppendLine();
            code.AppendLine("namespace Baer.Domain.IRepository." + tableEntity.OutputModel);
            code.AppendLine("{");
            GetAuthor(code, tableEntity);
            code.AppendLine("    public interface I" + tableEntity.ClassPrefix + "Repository : IRepositoryBase<" + tableEntity.ClassPrefix + "Entity>");
            code.AppendLine("    {");
            code.AppendLine("    }");
            code.AppendLine("}");
            return code.ToString();
        }

        private string GetRepositoryCode(string f_Id, TableEntity tableEntity)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine("using Baer.Data;");
            code.AppendLine("using Baer.Domain.Entity." + tableEntity.OutputModel + ";");
            code.AppendLine("using Baer.Domain.IRepository." + tableEntity.OutputModel + ";");
            code.AppendLine();
            code.AppendLine("namespace Baer.Repository." + tableEntity.OutputModel);
            code.AppendLine("{");
            GetAuthor(code, tableEntity);
            code.AppendLine("    public class " + tableEntity.ClassPrefix + "Repository : RepositoryBase<" + tableEntity.ClassPrefix + "Entity>, I" + tableEntity.ClassPrefix + "Repository");
            code.AppendLine("    {");
            code.AppendLine("    }");
            code.AppendLine("}");
            return code.ToString();
        }

        private string GetMappingCode(string f_Id, TableEntity tableEntity)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine("using Baer.Domain.Entity." + tableEntity.OutputModel + ";");
            code.AppendLine("using System.Data.Entity.ModelConfiguration;");
            code.AppendLine();
            code.AppendLine("namespace Baer.Mapping." + tableEntity.OutputModel);
            code.AppendLine("{");
            GetAuthor(code, tableEntity);
            code.AppendLine("    public class " + tableEntity.ClassPrefix + "Map : EntityTypeConfiguration<" + tableEntity.ClassPrefix + "Entity>");
            code.AppendLine("    {");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// 构造函数");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        public " + tableEntity.ClassPrefix + "Map()");
            code.AppendLine("        {");
            code.AppendLine("            this.ToTable(\"" + tableEntity.TableName + "\");");
            code.AppendLine("            this.HasKey(t => t.Id);");
            code.AppendLine("        }");
            code.AppendLine("    }");
            code.AppendLine("}");
            return code.ToString();
        }

        private string GetApplicationCode(string f_Id, TableEntity tableEntity)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine("using Baer.Code;");
            code.AppendLine("using Baer.Domain.Entity." + tableEntity.OutputModel + ";");
            code.AppendLine("using Baer.Domain.IRepository." + tableEntity.OutputModel + ";");
            code.AppendLine("using Baer.Repository." + tableEntity.OutputModel + ";");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine();
            code.AppendLine("namespace Baer.Application." + tableEntity.OutputModel);
            code.AppendLine("{");
            GetAuthor(code, tableEntity);
            code.AppendLine("    public class " + tableEntity.ClassPrefix + "App");
            code.AppendLine("    {");
            code.AppendLine("        private I" + tableEntity.ClassPrefix + "Repository service = new " + tableEntity.ClassPrefix + "Repository();");
            code.AppendLine();
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// 获取列表");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// <param name=\"pagination\">分页</param>");
            code.AppendLine("        /// <param name=\"keyword\">查询关键字（需要根据实际调整）</param>");
            code.AppendLine("        /// <returns>实体类列表</returns>");
            code.AppendLine("        public List<" + tableEntity.ClassPrefix + "Entity> GetList(Pagination pagination, string keyword = \"\")");
            code.AppendLine("        {");
            code.AppendLine("            var expression = ExtLinq.True<" + tableEntity.ClassPrefix + "Entity>();");
            code.AppendLine("            return service.FindList(expression, pagination);");
            code.AppendLine("        }");
            code.AppendLine();
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// 获取表单");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// <param name=\"f_Id\">查询主键</param>");
            code.AppendLine("        /// <returns>表单实体类</returns>");
            code.AppendLine("        public " + tableEntity.ClassPrefix + "Entity GetForm(string id)");
            code.AppendLine("        {");
            code.AppendLine("            return service.FindEntity(id);");
            code.AppendLine("        }");
            code.AppendLine();
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// 删除表单");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// <param name=\"f_Id\">删除主键</param>");
            code.AppendLine("        public void Delete(string id)");
            code.AppendLine("        {");
            code.AppendLine("            service.Delete(t => t.Id == id);");
            code.AppendLine("        }");
            code.AppendLine();
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// 提交表单");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// <param name=\"entity\">实体类</param>");
            code.AppendLine("        /// <param name=\"f_Id\">表单主键</param>");
            code.AppendLine("        public void Submit(" + tableEntity.ClassPrefix + "Entity entity, string id)");
            code.AppendLine("        {");
            code.AppendLine("            if (!string.IsNullOrEmpty(id))");
            code.AppendLine("            {");
            code.AppendLine("                entity.Modify(id);");
            code.AppendLine("                service.Update(entity);");
            code.AppendLine("            }");
            code.AppendLine("            else");
            code.AppendLine("            {");
            code.AppendLine("                entity.Create();");
            code.AppendLine("                service.Insert(entity);");
            code.AppendLine("            }");
            code.AppendLine("        }");
            code.AppendLine("    }");
            code.AppendLine("}");

            return code.ToString();
        }

        private string GetControllorCode(string f_Id, TableEntity tableEntity)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine("using Baer.Application." + tableEntity.OutputModel + ";");
            code.AppendLine("using Baer.Code;");
            code.AppendLine("using Baer.Domain.Entity." + tableEntity.OutputModel + ";");
            code.AppendLine("using System.Web.Mvc;");
            code.AppendLine();
            code.AppendLine("namespace Baer.Web.Areas." + tableEntity.OutputModel + ".Controllers");
            code.AppendLine("{");
            GetAuthor(code, tableEntity);
            code.AppendLine("    public class " + tableEntity.ClassPrefix + "Controller : ControllerBase");
            code.AppendLine("    {");
            code.AppendLine("        private " + tableEntity.ClassPrefix + "App app = new " + tableEntity.ClassPrefix + "App();");
            code.AppendLine();
            code.AppendLine("        #region ==========视图==========");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// 列表视图");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        public ActionResult " + tableEntity.ClassPrefix+ "Index()");
            code.AppendLine("        {");
            code.AppendLine("           return View();");
            code.AppendLine("        }");
            code.AppendLine();
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// 表单视图");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        public ActionResult " + tableEntity.ClassPrefix + "Form()");
            code.AppendLine("        {");
            code.AppendLine("           return View();");
            code.AppendLine("        }");
            code.AppendLine();
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// 明细视图");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        public ActionResult " + tableEntity.ClassPrefix + "Details()");
            code.AppendLine("        {");
            code.AppendLine("           return View();");
            code.AppendLine("        }");
            code.AppendLine("        #endregion");
            code.AppendLine();
            code.AppendLine("        #region ==========业务==========");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// 获取列表");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// <param name=\"pagination\">分页</param>");
            code.AppendLine("        /// <param name=\"keyword\">查询关键字（需要根据实际调整）</param>");
            code.AppendLine("        /// <returns>实体类列表</returns>");
            code.AppendLine("        [HttpGet]");
            code.AppendLine("        [HandlerAjaxOnly]");
            code.AppendLine("        public ActionResult GetGridJson(Pagination pagination, string keyword)");
            code.AppendLine("        {");
            code.AppendLine("            var data = new");
            code.AppendLine("            {");
            code.AppendLine("                rows = app.GetList(pagination, keyword),");
            code.AppendLine("                total = pagination.total,");
            code.AppendLine("                page = pagination.page,");
            code.AppendLine("                records = pagination.records");
            code.AppendLine("            };");
            code.AppendLine("            return Content(data.ToJson());");
            code.AppendLine("        }");
            code.AppendLine();
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// 获取表单");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// <param name=\"f_Id\">查询主键</param>");
            code.AppendLine("        /// <returns>表单实体类</returns>");
            code.AppendLine("        [HttpGet]");
            code.AppendLine("        [HandlerAjaxOnly]");
            code.AppendLine("        public ActionResult GetFormJson(string id)");
            code.AppendLine("        {");
            code.AppendLine("            var data = app.GetForm(id);");
            code.AppendLine("            return Content(data.ToJson());");
            code.AppendLine("        }");
            code.AppendLine();
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// 提交表单");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// <param name=\"entity\">实体类</param>");
            code.AppendLine("        /// <param name=\"f_Id\">表单主键</param>");
            code.AppendLine("        /// <returns></returns>");
            code.AppendLine("        [HttpPost]");
            code.AppendLine("        [HandlerAjaxOnly]");
            code.AppendLine("        [ValidateAntiForgeryToken]");
            code.AppendLine("        public ActionResult SubmitForm(" + tableEntity.ClassPrefix + "Entity entity, string id)");
            code.AppendLine("        {");
            code.AppendLine("            app.Submit(entity, id);");
            code.AppendLine("            return Success(\"操作成功。\");");
            code.AppendLine("        }");
            code.AppendLine();
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// 删除表单");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// <param name=\"f_Ids\">表单主键</param>");
            code.AppendLine("        /// <returns></returns>");
            code.AppendLine("        [HttpPost]");
            code.AppendLine("        [HandlerAjaxOnly]");
            code.AppendLine("        [HandlerAuthorize]");
            code.AppendLine("        [ValidateAntiForgeryToken]");
            code.AppendLine("        public ActionResult DeleteForm(string ids)");
            code.AppendLine("        {");
            code.AppendLine("            foreach (string id in ids.Split(','))");
            code.AppendLine("            {");
            code.AppendLine("                app.Delete(id);");
            code.AppendLine("            }");
            code.AppendLine("            return Success(\"删除成功。\");");
            code.AppendLine("        }");
            code.AppendLine("        #endregion");
            code.AppendLine("    }");
            code.AppendLine("}");
            return code.ToString();
        }

        private string GetRegisterCode(string f_Id, TableEntity tableEntity)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine("using System.Web.Mvc;");
            code.AppendLine();
            code.AppendLine("namespace Baer.Web.Areas.CottonManage");
            code.AppendLine("{");
            GetAuthor(code, tableEntity);
            code.AppendLine("    public class " + tableEntity.OutputModel + "AreaRegistration : AreaRegistration");
            code.AppendLine("    {");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// 获取路由");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        public override string AreaName");
            code.AppendLine("        {");
            code.AppendLine("            get");
            code.AppendLine("            {");
            code.AppendLine("                return \"" + tableEntity.OutputModel + "\";");
            code.AppendLine("            }");
            code.AppendLine("        }");
            code.AppendLine();
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// 注册表");
            code.AppendLine("        /// <summary>");
            code.AppendLine("        public override void RegisterArea(AreaRegistrationContext context)");
            code.AppendLine("        {");
            code.AppendLine("            context.MapRoute(");
            code.AppendLine("                this.AreaName + \"_Default\",");
            code.AppendLine("                this.AreaName + \"/{controller}/{action}/{id}\",");
            code.AppendLine("                new { area = this.AreaName, controller = \"Home\", action = \"Index\", id = UrlParameter.Optional },");
            code.AppendLine("                new string[] { \"Baer.Web.Areas.\" + this.AreaName + \".Controllers\" }");
            code.AppendLine("            );");
            code.AppendLine("        }");
            code.AppendLine("    }");
            code.AppendLine("}");
            return code.ToString();
        }

        private string GetListCode(string id, TableEntity tableEntity)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine("@{");
            code.AppendLine("    ViewBag.Title = \"Index\";");
            code.AppendLine("    Layout = \"~/Views/Shared/_Index.cshtml\";");
            code.AppendLine("}");
            code.AppendLine();
            code.AppendLine("<script>");
            code.AppendLine("    $(function () {");
            code.AppendLine("        gridList();");
            code.AppendLine();
            code.AppendLine("        var $cb = $('#cb_gridList');");
            code.AppendLine("        if ($cb != null) {");
            code.AppendLine("            $cb.parent().removeClass('ckbox');");
            code.AppendLine("        }");
            code.AppendLine("    })");
            code.AppendLine();
            code.AppendLine("    function gridList() {");
            code.AppendLine("        var $gridList = $('#gridList');");
            code.AppendLine("        $gridList.dataGrid({");
            code.AppendLine("            url: '/" + tableEntity.OutputModel + "/" + tableEntity.ClassPrefix + "/GetGridJson',");
            code.AppendLine("            height: $(window).height() - 128,");
            code.AppendLine("            colModel: [");
            code.AppendLine("                { label: '主键', name: 'Id', hidden: true, key: true },");
            GetListField(code, tableEntity);
            code.AppendLine("                {");
            code.AppendLine("                    label: '修改', name: '', width: 60, align: 'left',");
            code.AppendLine("                    formatter: function (val, obj, row, act) {");
            code.AppendLine("                        return '<a id=\"btnEdit\" authorize=\"yes\" class=\"btn\" onclick=\"operate(\\'' + row.Id + '\\')\"><i class=\"fa fa-pencil\"></i></a>'; ");
            code.AppendLine("                    }");
            code.AppendLine("                },");
            code.AppendLine("                {");
            code.AppendLine("                    label: '查看', name: '', width: 60, align: 'left',");
            code.AppendLine("                    formatter: function (val, obj, row, act) {");
            code.AppendLine("                        return '<a id=\"btnDetails\" authorize=\"yes\" class=\"btn\" onclick=\"details(\\'' + row.Id + '\\')\"><i class=\"fa fa-search-plus\"></i></a>'; ");
            code.AppendLine("                    }");
            code.AppendLine("                },");
            code.AppendLine("            ],");
            code.AppendLine("            pager: '#gridPager',");
            code.AppendLine("            sortname: 'CreatorTime asc',");
            code.AppendLine("            viewrecords: true,");
            code.AppendLine("            multiselect: true");
            code.AppendLine("        });");
            code.AppendLine();
            code.AppendLine("        $('#btnSearch').click(function () {");
            code.AppendLine("            $gridList.jqGrid('setGridParam', {");
            code.AppendLine("                postData: { keyword: $('#txtKeyword').val() },");
            code.AppendLine("            }).trigger('reloadGrid');");
            code.AppendLine("        });");
            code.AppendLine("    }");
            code.AppendLine();
            code.AppendLine("    function operate(id) {");
            code.AppendLine("        var title = !!id ? '修改' : '新增';");
            code.AppendLine("        $.modalOpen({");
            code.AppendLine("            id: 'Form',");
            code.AppendLine("            title: title,");
            code.AppendLine("            url: '/" + tableEntity.OutputModel + "/" + tableEntity.ClassPrefix + "/" + tableEntity.ClassPrefix + "Form?id=' + id,");
            code.AppendLine("            width: '550px',");
            code.AppendLine("            height: '" + (140 + tableEntity.SelectFormField.Split(',').Length * 40) + "px',");
            code.AppendLine("            btn: null");
            code.AppendLine("        });");
            code.AppendLine("    }");
            code.AppendLine();
            code.AppendLine("    function details(id) {");
            code.AppendLine("        $.modalOpen({");
            code.AppendLine("            id: 'Form',");
            code.AppendLine("            title: '查看',");
            code.AppendLine("            url: '/" + tableEntity.OutputModel + "/" + tableEntity.ClassPrefix + "/" + tableEntity.ClassPrefix + "Details?id=' + id,");
            code.AppendLine("            width: '550px',");
            code.AppendLine("            height: '" + (110 + tableEntity.SelectFormField.Split(',').Length * 40) + "px',");
            code.AppendLine("            btn: null");
            code.AppendLine("        });");
            code.AppendLine("    }");
            code.AppendLine();
            code.AppendLine("    function deletes() {");
            code.AppendLine("        var ids = $('#gridList').jqGrid('getGridParam', 'selarrrow');");
            code.AppendLine("        if (ids.length == 0) {");
            code.AppendLine("            this.alert('请选中项删除！');");
            code.AppendLine("            return;");
            code.AppendLine("        }");
            code.AppendLine();
            code.AppendLine("        $.deleteForm({");
            code.AppendLine("            url: '/" + tableEntity.OutputModel + "/" + tableEntity.ClassPrefix + "/DeleteForm',");
            code.AppendLine("            param: { ids: ids.toString() },");
            code.AppendLine("            success: function () {");
            code.AppendLine("                $.currentWindow().$('#gridList').trigger('reloadGrid');");
            code.AppendLine("            }");
            code.AppendLine("        })");
            code.AppendLine("    }");
            code.AppendLine("</script>");
            code.AppendLine();
            code.AppendLine("<div class='topPanel'>");
            code.AppendLine("    <div class='toolbar'>");
            code.AppendLine("        <div class='btn-group'>");
            code.AppendLine("            <a class='btn btn-primary' onclick='$.reload()'>");
            code.AppendLine("                <i class='fa fa-refresh fa-spin'></i>刷新");
            code.AppendLine("            </a>");
            code.AppendLine("        </div>");
            code.AppendLine("        <div class='btn-group'>");
            code.AppendLine("            <a id='btnDelete' authorize='yes' class='btn btn-primary dropdown-text' onclick='deletes();'>");
            code.AppendLine("               <i class='fa fa-trash-o'></i>删除");
            code.AppendLine("            </a>");
            code.AppendLine("        </div>");
            code.AppendLine("        <div class='btn-group'>");
            code.AppendLine("            <a id='btnAdd' authorize='yes' class='btn btn-primary dropdown-text' onclick='operate();'>");
            code.AppendLine("                <i class='fa fa-plus'></i>新增");
            code.AppendLine("            </a>");
            code.AppendLine("        </div>");
            code.AppendLine("        <script>$('.toolbar').authorizeButton()</script>");
            code.AppendLine("    </div>");
            code.AppendLine("    <div class='search'>");
            code.AppendLine("        <table>");
            code.AppendLine("            <tr>");
            code.AppendLine("                <td>");
            code.AppendLine("                    <div class='input-group'>");
            code.AppendLine("                        <input id='txtKeyword' type='text' class='form-control' placeholder='关键词' style='width: 200px; '>");
            code.AppendLine("                        <span class='input-group-btn'>");
            code.AppendLine("                            <button id='btnSearch' type='button' class='btn btn-primary'><i class='fa fa-search'></i></button>");
            code.AppendLine("                        </span>");
            code.AppendLine("                    </div>");
            code.AppendLine("                </td>");
            code.AppendLine("            </tr>");
            code.AppendLine("        </table>");
            code.AppendLine("    </div>");
            code.AppendLine("</div>");
            code.AppendLine("<div class='gridPanel'>");
            code.AppendLine("    <table id='gridList'></table>");
            code.AppendLine("    <div id='gridPager'></div>");
            code.AppendLine("</div>");
            return code.ToString();
        }

        private string GetFormCode(string f_Id, TableEntity tableEntity)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine("@{");
            code.AppendLine("    ViewBag.Title = \"Form\";");
            code.AppendLine("    Layout=\"~/Views/Shared/_Form.cshtml\";");
            code.AppendLine("}");
            code.AppendLine();
            code.AppendLine("<script>");
            code.AppendLine("    var id = $.request('id');");
            code.AppendLine("    $(function () {");
            code.AppendLine("        initControl();");
            code.AppendLine();
            code.AppendLine("        if (!!id) {");
            code.AppendLine("            $.ajax({");
            code.AppendLine("                url: '/" + tableEntity.OutputModel + "/" + tableEntity.ClassPrefix + "/GetFormJson',");
            code.AppendLine("                data: { id: id },");
            code.AppendLine("                dataType: 'json',");
            code.AppendLine("                async: false,");
            code.AppendLine("                success: function (data) {");
            code.AppendLine("                    $('#form1').formSerialize(data);");
            code.AppendLine("                }");
            code.AppendLine("            });");
            code.AppendLine("        }");
            code.AppendLine("    })");
            code.AppendLine();
            code.AppendLine("    function initControl() {");
            GetSelectInit(code, tableEntity);
            code.AppendLine("    }");
            code.AppendLine();
            code.AppendLine("    function submitForm() {");
            code.AppendLine("        var postData = $('#form1').formSerialize();");
            code.AppendLine("        $.submitForm({");
            code.AppendLine("           url: '/" + tableEntity.OutputModel + "/" + tableEntity.ClassPrefix + "/SubmitForm?id=' + id,");
            code.AppendLine("           param: postData,");
            code.AppendLine("           success: function () {");
            code.AppendLine("               $.currentWindow().$('#gridList').trigger('reloadGrid');");
            code.AppendLine("            }");
            code.AppendLine("        })");
            code.AppendLine("    }");
            code.AppendLine("</script>");
            code.AppendLine();
            code.AppendLine("<form id='form1'>");
            code.AppendLine("    <div style='margin: 10px;'>");
            code.AppendLine("        <div class='panel panel-default'>");
            code.AppendLine("            <div class='panel-body' style='width: 98%;'>");
            code.AppendLine("                <table class='form'>");
            GetFormField(code, tableEntity);
            code.AppendLine("                </table>");
            code.AppendLine("            </div>");
            code.AppendLine("        </div>");
            code.AppendLine("    </div>");
            code.AppendLine("    <div class='form-button'>");
            code.AppendLine("        <a id='btn_finish' class='btn btn-default' onclick='submitForm()'>保存</a>");
            code.AppendLine("    </div>");
            code.AppendLine("</form>");
            return code.ToString();
        }

        private string GetDetailsCode(string f_Id, TableEntity tableEntity)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine("@{");
            code.AppendLine("    ViewBag.Title = \"Details\";");
            code.AppendLine("    Layout = \"~/Views/Shared/_Details.cshtml\";");
            code.AppendLine("}");
            code.AppendLine();
            code.AppendLine("<script>");
            code.AppendLine("    var id = $.request('id');");
            code.AppendLine("    $(function () {");
            code.AppendLine("        initControl();");
            code.AppendLine();
            code.AppendLine("        $.ajax({");
            code.AppendLine("            url: '/" + tableEntity.OutputModel + "/" + tableEntity.ClassPrefix + "/GetFormJson',");
            code.AppendLine("            data: { id: id },");
            code.AppendLine("            dataType: 'json',");
            code.AppendLine("            async: false,");
            code.AppendLine("            success: function (data) {");
            code.AppendLine("                $('#form1').formSerialize(data);");
            code.AppendLine("                $('#form1').find('.form-control,select,input').attr('readonly', 'readonly');");
            code.AppendLine("                $('#form1').find('select').attr('disabled', 'disabled');");
            code.AppendLine("                $('#form1').find('div.ckbox label').attr('for', '');");
            code.AppendLine("            }");
            code.AppendLine("        })");
            code.AppendLine("    })");
            code.AppendLine();
            code.AppendLine("    function initControl() {");
            GetSelectInit(code, tableEntity);
            code.AppendLine("    }");
            code.AppendLine("</script>");
            code.AppendLine();
            code.AppendLine("<form id='form1'>");
            code.AppendLine("    <div style='margin: 10px;'>");
            code.AppendLine("        <div class='panel panel-default'>");
            code.AppendLine("            <div class='panel-body' style='width: 98%;'>");
            code.AppendLine("                <table class='form'>");
            GetDetailsField(code, tableEntity);
            code.AppendLine("                </table>");
            code.AppendLine("            </div>");
            code.AppendLine("        </div>");
            code.AppendLine("    </div>");
            code.AppendLine("</form>");
            return code.ToString();
        }

        private void GetAuthor(StringBuilder code, TableEntity tableEntity)
        {
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            code.AppendLine("    /// <summary>");
            code.AppendLine("    /// 创 建：" + LoginInfo.UserName);
            code.AppendLine("    /// 日 期：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            code.AppendLine("    /// 描 述：" + tableEntity.Description);
            code.AppendLine("    /// <summary>");
        }

        private void GetFieldDescription(StringBuilder code, string description)
        {
            code.AppendLine("        /// <summary>");
            code.AppendLine("        /// " + description);
            code.AppendLine("        /// <summary>");
        }

        private void GetListField(StringBuilder code, TableEntity tableEntity)
        {
            if (!string.IsNullOrEmpty(tableEntity.SelectListField))
            {
                DataView dv = DbHelper.ExecuteToDataView("select FieldName,FieldChineseName,ShowType,DataSource from Sys_TableFieldInfo where FieldName in ('" + tableEntity.SelectListField.Replace(",", "','") + "')");
                foreach (DataRowView drv in dv)
                {
                    if (Convert.ToString(drv["ShowType"]) == "2")
                    {
                        code.AppendLine("                {");
                        code.AppendLine("                   label: '" + drv["FieldChineseName"] + "', name: '" + drv["FieldName"] + "', width: 80, align: 'left',");
                        code.AppendLine("                   formatter: function (cellvalue, options, rowObject) {");
                        code.AppendLine("                       return top.clients.dataItems['" + drv["DataSource"] + "'][cellvalue] == null ? '' : top.clients.dataItems['" + drv["DataSource"] + "'][cellvalue];");
                        code.AppendLine("                   }");
                        code.AppendLine("                },");
                    }
                    else
                    {
                        code.AppendLine("                { label: '" + drv["FieldChineseName"] + "', name: '" + drv["FieldName"] + "', width: 80, align: 'left' },");
                    }
                }
            }
        }

        private void GetFormField(StringBuilder code, TableEntity tableEntity)
        {
            if (!string.IsNullOrEmpty(tableEntity.SelectFormField))
            {
                DataView dv = DbHelper.ExecuteToDataView("select FieldName,FieldChineseName,ShowType from Sys_TableFieldInfo where FieldName in ('" + tableEntity.SelectFormField.Replace(",", "','") + "')");
                foreach (DataRowView drv in dv)
                {
                    code.AppendLine("                    <tr>");
                    code.AppendLine("                        <th class='formTitle'>" + drv["FieldChineseName"] + "</th>");
                    code.AppendLine("                        <td class='formValue'>");
                    switch (Convert.ToInt32(drv["ShowType"]))
                    {
                        case 1:
                            code.AppendLine("                            <input id='" + drv["FieldName"] + "' type='text' class='form-control input-wdatepicker' onfocus='WdatePicker()' />");
                            break;
                        case 2:
                            code.AppendLine("                            <select id='" + drv["FieldName"] + "' class='form-control'>");
                            code.AppendLine("                               <option value=''>==请选择==</option>");
                            code.AppendLine("                            </select>");
                            break;
                        case 0:
                        default:
                            code.AppendLine("                            <input id='" + drv["FieldName"] + "' type='text' class='form-control'/>");
                            break;
                    }
                    
                    code.AppendLine("                        </td>");
                    code.AppendLine("                    </tr>");
                }
            }
        }

        private void GetDetailsField(StringBuilder code, TableEntity tableEntity)
        {
            if (!string.IsNullOrEmpty(tableEntity.SelectFormField))
            {
                DataView dv = DbHelper.ExecuteToDataView("select FieldName,FieldChineseName,ShowType from Sys_TableFieldInfo where FieldName in ('" + tableEntity.SelectFormField.Replace(",", "','") + "')");
                foreach (DataRowView drv in dv)
                {
                    code.AppendLine("                    <tr>");
                    code.AppendLine("                        <th class='formTitle'>" + drv["FieldChineseName"] + "</th>");
                    code.AppendLine("                        <td class='formValue'>");
                    switch (Convert.ToInt32(drv["ShowType"]))
                    {
                        case 1:
                            code.AppendLine("                            <input id='" + drv["FieldName"] + "' type='text' class='form-control input-wdatepicker' />");
                            break;
                        case 2:
                            code.AppendLine("                            <select id='" + drv["FieldName"] + "' class='form-control'>");
                            code.AppendLine("                               <option value=''>==请选择==</option>");
                            code.AppendLine("                            </select>");
                            break;
                        case 0:
                        default:
                            code.AppendLine("                            <input id='" + drv["FieldName"] + "' type='text' class='form-control'/>");
                            break;
                    }

                    code.AppendLine("                        </td>");
                    code.AppendLine("                    </tr>");
                }
            }
        }

        private void GetSelectInit(StringBuilder code, TableEntity tableEntity)
        {
            if (!string.IsNullOrEmpty(tableEntity.SelectFormField))
            {
                DataView dv = DbHelper.ExecuteToDataView("select FieldName,DataSource from Sys_TableFieldInfo where FieldName in ('" + tableEntity.SelectFormField.Replace(",", "','") + "') and ShowType=2");
                foreach (DataRowView drv in dv)
                {
                    code.AppendLine("       $('#" + drv["FieldName"] + "').bindSelect({");
                    code.AppendLine("           url: '/SystemManage/ItemsData/GetSelectJson',");
                    code.AppendLine("           param: { enCode: '" + drv["DataSource"] + "' },");
                    code.AppendLine("       });");
                }
            }
        }
        #endregion

        #region ==========文件==========
        private void CreateFile(string path, string content)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.Write(content);
            }
        }

        private void IncludingCsproj(string path, string content, string item = "Compile")
        {
            ProjectCollection pc = new ProjectCollection();
            //加载cjproj文件
            Project proj = pc.LoadProject(path);

            var projectItems = proj.GetItems("Compile");
            switch (item)
            {
                case "Compile":
                    //默认值
                    break;
                case "Content":
                    projectItems = proj.GetItems("Content");
                    break;
                default:
                    //判断不了什么类型的，不写数据
                    return;
            }

            //判断是否已存在
            foreach (ProjectItem projectItem in projectItems)
            {
                if (projectItem.EvaluatedInclude == content.Trim('\\'))
                {
                    return;
                }
            }

            //新增配置
            proj.AddItem(item, content.Trim('\\'));

            //编辑与保存
            proj.Build();
            proj.Save();
        }
        #endregion
    }
}
