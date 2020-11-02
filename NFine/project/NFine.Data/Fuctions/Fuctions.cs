using NFine.Code;
using NFine.Data.Extensions;

namespace NFine.Data
{
    public class Fuctions
    {
        public static bool ChangeStep(string changeInfo, string  type)
        {
            FRow fRow = new FRow("ChangeInfo");
            fRow["ChangeInfo"] = OperatorProvider.Provider.GetCurrent().UserName + "操作：" + changeInfo;
            fRow["Type"] = type;
            return fRow.Insert();
        }
    }
}
