using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ServerGame106.Data;
using ServerGame106.Models;
using ServerGame106.DTO;
using ServerGame106.ViewModel;

namespace ServerGame106.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIGameController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        protected ResponseApi _response;
        private readonly UserManager<ApplicationUser> _userManager;

        public APIGameController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _response = new();
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Server is running");
        }

        // Bài 2: Các API lấy dữ liệu
        [HttpGet("GetAllGameLevel")]
        public async Task<IActionResult> GetAllGameLevel()
        {
            try
            {
                var gameLevel = await _db.GameLevels.ToListAsync();
                _response.IsSuccess = true;
                _response.Notification = "Lấy dữ liệu thành công";
                _response.Data = gameLevel;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lỗi";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpGet("GetAllQuestionGame")]
        public async Task<IActionResult> GetAllQuestionGame()
        {
            try
            {
                var questionGame = await _db.Questions.ToListAsync();
                _response.IsSuccess = true;
                _response.Notification = "Lấy dữ liệu thành công";
                _response.Data = questionGame;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lỗi";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpGet("GetAllRegion")]
        public async Task<IActionResult> GetAllRegion()
        {
            try
            {
                var region = await _db.Regions.ToListAsync();
                _response.IsSuccess = true;
                _response.Notification = "Lấy dữ liệu thành công";
                _response.Data = region;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lỗi";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        // Bài 4: Lấy câu hỏi theo Level
        [HttpGet("GetAllQuestionGameByLevel/{levelId}")]
        public async Task<IActionResult> GetAllQuestionGameByLevel(int levelId)
        {
            try
            {
                var questionGame = await _db.Questions.Where(x => x.LevelId == levelId).ToListAsync();
                _response.IsSuccess = true;
                _response.Notification = "Lấy dữ liệu thành công";
                _response.Data = questionGame;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lỗi";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        // Bài 3: Đăng ký & Đăng nhập
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                var user = new ApplicationUser
                {
                    Email = registerDTO.Email,
                    UserName = registerDTO.Email,
                    Name = registerDTO.Name,
                    Avatar = registerDTO.LinkAvatar,
                    RegionId = registerDTO.RegionId
                };

                var result = await _userManager.CreateAsync(user, registerDTO.Password);

                if (result.Succeeded)
                {
                    _response.IsSuccess = true;
                    _response.Notification = "Đăng ký thành công";
                    _response.Data = user;
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Notification = "Đăng ký thất bại";
                    _response.Data = result.Errors;
                    return BadRequest(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lỗi";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Microsoft.AspNetCore.Identity.Data.LoginRequest loginRequest)
        {
            try
            {
                var email = loginRequest.Email;
                var password = loginRequest.Password;

                var user = await _userManager.FindByEmailAsync(email);

                if (user != null && await _userManager.CheckPasswordAsync(user, password))
                {
                    _response.IsSuccess = true;
                    _response.Notification = "Đăng nhập thành công";
                    _response.Data = user;
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Notification = "Đăng nhập thất bại";
                    _response.Data = null;
                    return BadRequest(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lỗi";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        // Bài 5: Lưu kết quả
        [HttpPost("SaveResult")]
        public async Task<IActionResult> SaveResult(LevelResultDTO levelResult)
        {
            try
            {
                var levelResultSave = new LevelResult
                {
                    UserId = levelResult.UserId,
                    LevelId = levelResult.LevelId,
                    Score = levelResult.Score,
                    CompletionDate = DateOnly.FromDateTime(DateTime.Now)
                };

                await _db.LevelResults.AddAsync(levelResultSave);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Notification = "Lưu kết quả thành công";
                _response.Data = levelResult;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lỗi";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        // Bài 6: Xếp hạng (Rating)
        [HttpGet("Rating/{idRegion}")]
        public async Task<IActionResult> Rating(int idRegion)
        {
            try
            {
                RatingVM ratingVM = new RatingVM();
                List<ApplicationUser> userList = new List<ApplicationUser>();
                List<LevelResult> resultList = new List<LevelResult>();

                if (idRegion > 0)
                {
                    var region = await _db.Regions.FirstOrDefaultAsync(x => x.RegionId == idRegion);
                    if (region == null)
                    {
                        _response.IsSuccess = false;
                        _response.Notification = "Không tìm thấy khu vực";
                        return BadRequest(_response);
                    }
                    ratingVM.NameRegion = region.Name;
                    userList = await _db.Users.Where(x => x.RegionId == idRegion).ToListAsync();
                    var userIds = userList.Select(x => x.Id).ToList();
                    resultList = await _db.LevelResults.Where(x => userIds.Contains(x.UserId)).ToListAsync();
                }
                else
                {
                    ratingVM.NameRegion = "Tất cả";
                    userList = await _db.Users.ToListAsync();
                    resultList = await _db.LevelResults.ToListAsync();
                }

                ratingVM.userResultSums = new List<UserResultSum>();
                foreach (var item in userList)
                {
                    var sumScore = resultList.Where(x => x.UserId == item.Id).Sum(x => x.Score);
                    var sumLevel = resultList.Where(x => x.UserId == item.Id).Count();

                    UserResultSum userResultSum = new UserResultSum
                    {
                        NameUser = item.Name,
                        SumScore = sumScore,
                        SumLevel = sumLevel
                    };
                    ratingVM.userResultSums.Add(userResultSum);
                }

                _response.IsSuccess = true;
                _response.Notification = "Lấy dữ liệu thành công";
                _response.Data = ratingVM;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lỗi";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        // --- Bài 7: Lấy thông tin User (MỚI THÊM) ---
        [HttpGet("GetUserInformation/{userId}")]
        public async Task<IActionResult> GetUserInformation(string userId)
        {
            try
            {
                // Tìm user trong DB
                var user = await _db.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.Notification = "Không tìm thấy người dùng";
                    _response.Data = null;
                    return BadRequest(_response);
                }

                // Tạo ViewModel để trả về
                UserInformationVM userInformationVM = new UserInformationVM();
                userInformationVM.Name = user.Name;
                userInformationVM.Email = user.Email;
                userInformationVM.avatar = user.Avatar;

                // Lấy tên vùng miền
                userInformationVM.Region = await _db.Regions
                                            .Where(x => x.RegionId == user.RegionId)
                                            .Select(x => x.Name)
                                            .FirstOrDefaultAsync();

                _response.IsSuccess = true;
                _response.Notification = "Lấy dữ liệu thành công";
                _response.Data = userInformationVM;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lỗi";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }
    }
}