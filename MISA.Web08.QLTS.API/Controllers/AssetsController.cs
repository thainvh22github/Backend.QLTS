// using: sử dụng các hàm
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web08.QLTS.API.Entities;
using MISA.Web08.QLTS.API.Entities.DTO;
using MISA.Web08.QLTS.API.Enums;

namespace MISA.Web08.QLTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        /// <summary>
        /// API lấy danh sách toàn bộ nhân viên
        /// </summary>
        /// <returns>Lấy danh sách toàn bộ nhân viên</returns>
        /// Author: NVHThai (16/09/2022)
        [HttpGet]
        [Route("")]
        public List<Asset> GetAllAssets()
        {
            return new List<Asset>
            {
                new Asset
                {
                    AssetID = Guid.NewGuid(),
                    AssetCode = "TS001",
                    AssetName = "Xe honda",
                    Gender = Gender.Male,
                }
            };
        }


        /// <summary>
        /// Api lấy thông tin 1 tài sản theo id
        /// </summary>
        /// <param name="assetID">ID tài sản muốn lấy</param>
        /// <returns>Thông tin 1 tài sản</returns>
        /// Author: NVHThai (16/09/2022)
        [HttpGet]
        [Route("{assetID}")]
        public Asset GetAssetByID([FromRoute] Guid assetID)
        {
            return new Asset
            {
                AssetID = assetID,
                AssetCode = "TS001",
                AssetName = "Xe honda",
                Gender = Gender.Male,
            };
        }


        [HttpGet("filter")]
        public PaggingData FilterAssets([FromQuery] string keword, [FromQuery] Guid assetCategoryID,
            [FromQuery] Guid departmentID, [FromQuery] int limit, [FromQuery] int offset)
        {
            return new PaggingData
            {
                Data = new List<Asset>
                {
                    new Asset
                    {
                        AssetID = Guid.NewGuid(),
                        AssetCode = "TS001",
                        AssetName = "Xe honda",
                        Gender = Gender.Male,
                    }
                }
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InsertAsset([FromBody] Asset asset)
        {
            return StatusCode(StatusCodes.Status201Created, Guid.NewGuid());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        [HttpPut("{assetID}")]
        public IActionResult UpdateAsset([FromRoute] Guid assetID, [FromBody] Asset asset)
        {
            return StatusCode(StatusCodes.Status200OK, assetID);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        [HttpDelete("{assetID}")]
        public IActionResult DeleteAsset([FromRoute] Guid assetID)
        {
            return StatusCode(StatusCodes.Status200OK, assetID);
        }


        [HttpPost("batch-delete")]
        public IActionResult DeleteMutipleAssets([FromBody] List<string> assetIDs)
        {
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}
