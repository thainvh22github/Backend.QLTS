namespace MISA.Web08.QLTS.API.Entities.DTO
{
    public class PaggingData<Assets>
    {
        public List<Assets> Data { get; set; } = new List<Assets>();

        public int TotalCount { get; set; }
    }
}
