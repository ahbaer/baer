﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.SystemManage
{
    public class TableEntity : IEntity<FileEntity>, ICreationAudited, IModificationAudited, IDeleteAudited
    {
        public string F_Id { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
        public bool? F_DeleteMark { get; set; }

        //我的属性
        public string TableName { get; set; }//表名
        public string TableChineseName { get; set; }//表名（中文）
        public bool? AllowCreate { get; set; }//允许新增
        public bool? AllowEdit { get; set; }//允许修改
        public bool? AllowDelete { get; set; }//允许删除
        public string ClassPrefix { get; set; }//自动生成代码类名前缀
        public string OutputModel { get; set; }//自动生成代码输出模块
        public string SelectListField { get; set; }//自动生成代码列表选中字段
        public string SelectFormField { get; set; }//自动生成代码表单选中字段
        public string F_Description { get; set; }//备注
    }
}