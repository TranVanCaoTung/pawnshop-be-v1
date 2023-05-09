using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PawnShopBE.Core.Const;
using PawnShopBE.Core.DTOs;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Helpers;
using PawnShopBE.Infrastructure.Helpers;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UserService : IUserService
    {
        public IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private DbContextClass _dbContextClass;
        private IUserRepository _userRepository;
        private IUserBranchService _userBranchService;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, DbContextClass dbContextClass, IUserRepository userRepository, IUserBranchService userBranchService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dbContextClass = dbContextClass;
            _userRepository = userRepository;
            _userBranchService = userBranchService;
        }
        public async Task<bool> CreateUser(UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);
            if (user != null)
            {

                user.CreateTime = DateTime.Now;
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                // Create User
                await _unitOfWork.Users.Add(user);
                
                var result = _unitOfWork.Save();

                if (result > 0)
                {
                    // Create UserBranch
                    var userBranch = new UserBranch();
                    userBranch.UserId = user.UserId;
                    userBranch.BranchId = userDTO.BranchId;
                    await _userBranchService.CreateUserBranch(userBranch);
                    return true;
                }
                    
            }
            return false;
        }

        public async Task<bool> SendEmail(string email)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(email);
                var userBranch =  _dbContextClass.UserBranches.FirstOrDefault(u => u.UserId == user.UserId);
                if (user == null)
                {
                    return false;
                }
                string _gmail = "hethongpawns@gmail.com";
                string _password = "fnblmxkfeaeilbxs";
                string randomPassword = HelperFuncs.GeneratePassword(10);

                string sendto = user.Email;
                string subject = "[PAWNSHOP] - Khôi phục mật khẩu";
                string content = "Mật khẩu mới cho tài khoản đăng nhập " + user.UserName + " : " + randomPassword + ".\nĐường link quay lại trang đăng nhập: ";

                // Create random password
                //set new password
                user.Password = randomPassword;
                //update password
                await UpdateUser(user, userBranch.BranchId);

                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                //set property for email you want to send
                mail.From = new MailAddress(_gmail);
                mail.To.Add(sendto);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = content;
                mail.Priority = MailPriority.High;
                //set smtp port
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                //set gmail pass sender
                smtp.Credentials = new NetworkCredential(_gmail, _password);
                smtp.EnableSsl = true;
                smtp.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public async Task<bool> DeleteUser(Guid userId)
        {
            if (userId != null)
            {
                var user = await _unitOfWork.Users.GetById(userId);
                if (user != null)
                {
                    _unitOfWork.Users.Delete(user);
                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        public async Task<IEnumerable<User>> GetAllUsers(int num)
        {
            var userList = await _unitOfWork.Users.GetAll();
            if (num == 0)
            {
                return userList;
            }
            var result = await _unitOfWork.Users.TakePage(num, userList);
            return result;
        }

        public async Task<User> GetUserById(Guid userId)
        {
            if (userId != null)
            {
                var user = await _unitOfWork.Users.GetById(userId);
                if (user != null)
                {
                    return user;
                }
            }
            return null;
        }

        public async Task<bool> UpdateUser(User user, int branchId)
        {
            if (user != null)
            {
                var userUpdate = await _unitOfWork.Users.GetById(user.UserId);
                if (userUpdate != null)
                {
                    userUpdate.UserName = user.UserName;
                    userUpdate.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    userUpdate.Status = user.Status;
                    userUpdate.Email= user.Email;
                    userUpdate.Phone = user.Phone;
                    userUpdate.Address= user.Address;
                    userUpdate.FullName= user.FullName;
                    userUpdate.Role= user.Role;
                    userUpdate.UpdateTime = DateTime.Now;
                    var userBranch = new UserBranch();
                    userBranch.UserId = user.UserId;
                    userBranch.BranchId = branchId;
                    _userBranchService.UpdateUserBranch(userBranch);
                    _unitOfWork.Users.Update(userUpdate);

                    var result = _unitOfWork.Save();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        public async Task<User> GetAdmin(int role)
        {
            var admin = new User();
            admin = (role == (int)RoleConst.ADMIN) ? _dbContextClass.User.FirstOrDefault(a => a.RoleId == role) : null;
            return admin;
        }
    }
}
