using MySqlX.XDevAPI.Common;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Core.Models;
using PawnShopBE.Infrastructure.Helpers;
using PawnShopBE.Infrastructure.Repositories;
using Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class DiaryImgService : IDiaryImgService
    {
        private readonly IUnitOfWork _unit;
        private readonly IDiaryImgRepository _diaryImgRepository;
        public DiaryImgService(IUnitOfWork unitOfWork, IDiaryImgRepository diaryImgRepository)
        {
            _unit = unitOfWork;
            _diaryImgRepository = diaryImgRepository;   
        }

        public async Task<bool> CreateDiariesImg(int interestDiaryId, List<string> prooImgs)
        {
            if (prooImgs != null)
            {
                List<DiaryImg> diaryImgList = new List<DiaryImg>();
                foreach (var img in prooImgs)
                {
                    var diaryImg = new DiaryImg();
                    diaryImg.InterestDiaryId = interestDiaryId;
                    diaryImg.ProofImg = img;
                    diaryImg.PaidDate = DateTime.Now;
                    diaryImgList.Add(diaryImg);
                    await _unit.DiaryImgs.Add(diaryImg);

                }

                var result = _unit.Save();
                if (result > 0) return true;

            }
            return false;
        }

        public async Task<IEnumerable<DiaryImg?>> GetDiariesImg(int interestDiaryId)
        {
            try
            {
                var diaryImgList = await _unit.DiaryImgs.GetDiaryImgByInterestDiaryId(interestDiaryId);
                if (diaryImgList != null)
                {
                    return diaryImgList;
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString);
            }
            return null;
        }

        public async Task<bool> UpdateDiariesImg(int interestDiaryId)
        {
            //var diary = _diaryImgRepository.GetDiaryImgByInterestDiaryId(interestDiaryId);
            //if (diary != null)
            //{
            //    diary
            //}
            //_unit.DependentPeople.Update(dependentUpdate);
            //var result = _unit.Save();
            return false;
        }
    }
}
