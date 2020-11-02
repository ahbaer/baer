namespace NFine.Domain._03_Entity.Application
{
    /// <summary>
    /// 期货行情（批量查询）
    /// </summary>
    public class Comrms
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public Obj[] Obj { get; set; }
    }

    public class Obj
    {
        public string TS { get; set; }
        public string N { get; set; }
        public string M { get; set; }
        public string S { get; set; }
        public string C { get; set; }
        public string FS { get; set; }
        public float P { get; set; }
        public float NV { get; set; }
        public int Tick { get; set; }
        public float B1 { get; set; }
        public int B2 { get; set; }
        public int B3 { get; set; }
        public int B4 { get; set; }
        public int B5 { get; set; }
        public int B1V { get; set; }
        public int B2V { get; set; }
        public int B3V { get; set; }
        public int B4V { get; set; }
        public int B5V { get; set; }
        public float S1 { get; set; }
        public int S2 { get; set; }
        public int S3 { get; set; }
        public int S4 { get; set; }
        public int S5 { get; set; }
        public int S1V { get; set; }
        public int S2V { get; set; }
        public int S3V { get; set; }
        public int S4V { get; set; }
        public int S5V { get; set; }
        public float ZT { get; set; }
        public float DT { get; set; }
        public float O { get; set; }
        public float H { get; set; }
        public float L { get; set; }
        public float YC { get; set; }
        public float A { get; set; }
        public int V { get; set; }
        public int OV { get; set; }
        public int IV { get; set; }
        public int SY { get; set; }
        public int SJ { get; set; }
        public int HS { get; set; }
        public float ZS { get; set; }
        public float LS { get; set; }
        public float Z { get; set; }
        public float Z2 { get; set; }
        public float VF { get; set; }
        public float ZF { get; set; }
        public float JS { get; set; }
        public float YJS { get; set; }
        public int HD { get; set; }
        public int YHD { get; set; }
        public float AVP { get; set; }
        public int SY2 { get; set; }
        public int QJ { get; set; }
        public string N2 { get; set; }
        public string QR { get; set; }
    }

}
