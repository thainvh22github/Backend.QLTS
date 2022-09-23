namespace MISA.Web08.QLTS.API.Entities
{
    public class Department
    {
        /// <summary>
        /// id phòng ban
        /// </summary>
        public Guid DepartmentID { get; set; }

        /// <summary>
        /// mã phòng ban
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        /// tên phòng ban
        /// </summary>
        public string DepartmentName { get; set; }
    }
}
