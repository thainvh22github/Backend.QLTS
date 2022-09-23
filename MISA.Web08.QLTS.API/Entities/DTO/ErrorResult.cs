namespace MISA.Web08.QLTS.API.Entities.DTO
{
    public class ErrorResult
    {
        public string ErrorCode { get; set; }

        public string DevMsg { get; set; }

        public string UserMsg { get; set; }

        public string MoreInfo { get; set; }

        public string TraceId { get; set; }
    }
}
