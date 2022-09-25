using MISA.Web08.QLTS.API.Attributes;

namespace MISA.Web08.QLTS.API.Entities
{
    public class Assets
    {
        /// <summary>
        /// ID tài sản
        /// </summary>
        [PrimaryKey]
        public Guid fixed_asset_id { get; set; }

        /// <summary>
        /// Mã tài sản
        /// </summary>
        [IsNotNullOrEmpty("Mã tài sản không được để trống")]
        public string fixed_asset_code { get; set; }

        /// <summary>
        /// tên tài sản
        /// </summary>
        [IsNotNullOrEmpty("tên tài sản không được để trống")]
        public string fixed_asset_name { get; set; }


        /// <summary>
        /// Id loại tài sản
        /// </summary>
        public Guid department_id { get; set; }

        /// <summary>
        /// mã loại tài sản
        /// </summary>
        [IsNotNullOrEmpty("Mã phòng ban không được để trống")]
        public string department_code { get; set; }

        /// <summary>
        /// tên loại tài sản
        /// </summary>
        public string department_name { get; set; }

        /// <summary>
        /// id phòng ban
        /// </summary>
        public Guid fixed_asset_category_id { get; set; }

        /// <summary>
        /// mã phòng ban
        /// </summary>
        [IsNotNullOrEmpty("Mã loại tài sản không được để trống")]
        public string fixed_asset_category_code { get; set; }

        /// <summary>
        /// tên phòng ban
        /// </summary>
        public string fixed_asset_category_name { get; set; }

        /// <summary>
        /// ngày mua
        /// </summary>
        [IsNotNullOrEmpty("Ngày mua không được để trống")]
        public DateTime purchase_date { get; set; }


        /// <summary>
        /// nguyên giá
        /// </summary>
        [IsNotNullOrEmpty("Nguyên giá không được để trống")]
        public float cost { get; set; }

        /// <summary>
        /// số lượng
        /// </summary>
        [IsNotNullOrEmpty("Số lượng không được để trống")]
        public int quantity { get; set; }

        /// <summary>
        /// tỷ lệ hao mòn
        /// </summary>
        [IsNotNullOrEmpty("tỷ lệ hao mòn không được để trống")]
        public float depreciation_rate { get; set; }

        /// <summary>
        /// năm theo dõi
        /// </summary>
        public int tracked_year { get; set; }


        /// <summary>
        /// số năm sử dụng
        /// </summary>
        [IsNotNullOrEmpty("số năm sử dụng không được để trống")]
        public int life_time { get; set; }

        /// <summary>
        /// ngày bắt đầu sử dụng
        /// </summary>
        [IsNotNullOrEmpty("ngày bắt đầu sử dụng không được để trống")]
        public DateTime production_date { get; set; }

        /// <summary>
        /// người tạo bảng
        /// </summary>
        public string created_by { get; set; }

        /// <summary>
        /// ngày tạo bảng
        /// </summary>
        public DateTime created_date { get; set; }

        /// <summary>
        /// người sửa bảng
        /// </summary>
        public string modified_by { get; set; }

        /// <summary>
        /// ngày sửa bảng
        /// </summary>
        public DateTime modified_date { get; set; }
    }
}
