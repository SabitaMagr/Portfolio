﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Portfolio.Domain.Entities;
using Portfolio.Domain.Entities.User;
using Portfolio.Domain.HelperClass;
using Portfolio.Domain.Interfaces;
using Portfolio.Infrastructure.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Portfolio.Infrastructure.Repository
{
    public class UserRepository : IUserRepo
    {
        PortfolioDbContext _dbContext;
        public UserRepository(PortfolioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool AddUser(SignUpModel model)
        {
            try
            {
                int maxId = GetMaxId<UserTbl>("Id");
                string formattedDate = DateTime.Now.ToString("dd-MMM-yyyy");
                string hashPassword = StaticHelper.EncryptString(model.Password);
                var newUser = new UserTbl
                {
                    Id  =maxId,
                    Full_name = model.FullName,
                    User_name = model.Username,
                    Password = hashPassword,
                    Created_dt = DateTime.ParseExact(formattedDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    Status = "E"
                };
                _dbContext.UserTbl.Add(newUser);
                _dbContext.SaveChanges();
                return true;

            }catch(Exception )
            {
                 return false;
            }
        }
        public UserTbl ValidateUser(LoginModel model)
        {
            var user = _dbContext.UserTbl.FirstOrDefault(u => u.User_name == model.UserName && u.Status == "E");
            if(user!=null)
            {
                var decryptedPassword = StaticHelper.DecryptString(user.Password);
                if (decryptedPassword == model.Password)
                {
                    return user; 
                }
            }
            return null;
        }
        #region Skills
        public bool AddSkills(List<string> skills,string token)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    string formattedDate = DateTime.Now.ToString("dd-MMM-yyyy");
                    int userId = int.TryParse(StaticHelper.GetDetail(token, "Id"), out var parsedId) ? parsedId : 0;
                    foreach (var data in skills)
                    {
                        if (!string.IsNullOrEmpty(data))
                        {
                            int maxId = GetMaxId<Skills>("ID");
                            var skillData = new Skills
                            {
                                ID = maxId,
                                Skill = data,
                                Created_by = userId,
                                Created_dt = DateTime.ParseExact(formattedDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                                Status = "E"
                            };
                            _dbContext.Skills.Add(skillData);
                            _dbContext.SaveChanges();
                        }
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception )
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public List<SkillDetail> getSkills( string token)
        {
            try
            {
                int userId = int.TryParse(StaticHelper.GetDetail(token, "Id"), out var parsedId) ? parsedId : 0;
                var skills=_dbContext.Skills
                    .Where(s=>s.Created_by==userId && s.Status=="E")
                    .Join(_dbContext.UserTbl,
                    s=>s.Created_by,u=>u.Id,
                    (s,u)=>new SkillDetail
                    {
                        Id=s.ID,
                        Skill=s.Skill,
                        Created_by=u.Full_name,
                        Created_dt = s.Created_dt.HasValue ? s.Created_dt.Value.ToString("dd-MMM-yyyy") : "N/A"
                    })
                    .OrderByDescending(s => s.Id)
                    .ToList();
                    return skills;
            }
            catch(Exception )
            {
                return new List<SkillDetail>();
            }

        }
        public bool UpdateSkills(int id,string token)
        {
            try
            {
                string formattedDate = DateTime.Now.ToString("dd-MMM-yyyy");
                int userId = int.TryParse(StaticHelper.GetDetail(token, "Id"), out var parsedId) ? parsedId : 0;
                var skill = _dbContext.Skills.Find(id);
                if (skill == null)
                {
                    return false;
                }
                skill.Status = "D";
                skill.Modified_by = userId;
                skill.Modified_dt = DateTime.ParseExact(formattedDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }
        public Skills GetSkillById(int id)
        {
            try
            {
                var skill = _dbContext.Skills
                    .Where(s => s.ID == id)
                    .Select(s => new Skills { ID = s.ID, Skill = s.Skill })
                    .FirstOrDefault();

                return skill;
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (logging, error handling, etc.)
                throw new Exception("Error fetching skill by ID", ex);
            }
        }

        public bool UpdateSkillbyId(List<string> skills, string token,int id)
        {
            try
            {
                string formattedDate = DateTime.Now.ToString("dd-MMM-yyyy");
                int userId = int.TryParse(StaticHelper.GetDetail(token, "Id"), out var parsedId) ? parsedId : 0;
                var skill = _dbContext.Skills.Find(id);
                foreach (var data in skills)
                {
                    if (!string.IsNullOrEmpty(data))
                    {
                        skill.Skill = data;
                        skill.Modified_by = userId;
                        skill.Modified_dt = DateTime.ParseExact(formattedDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        _dbContext.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }
        #endregion Skills
        #region Personal Details
        public bool AddData(PersonalDtl data,string token)
        {
            try
            {
                string formattedDate = DateTime.Now.ToString("dd-MMM-yyyy");
                int userId = int.TryParse(StaticHelper.GetDetail(token, "Id"), out var parsedId) ? parsedId : 0;
                if (data.UserId== 0)
                {
                    int maxId = GetMaxId<PersonalDetails>("UserId");
                    var newData = new PersonalDetails()
                    {
                        UserId= maxId,
                        FullName = data.FullName,
                        MobileNo = data.MobileNo,
                        Address= data.Address,
                        Email = data.Email,
                        Profile = data.Profile,
                        About  = data.About,
                        Summary= data.Summary,
                        Status = "E",
                        Created_by = userId,
                        Created_dt = DateTime.ParseExact(formattedDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    };
                    _dbContext.PersonalDetails.Add(newData);
                    _dbContext.SaveChanges();
                }
                else
                {
                    var personalData = _dbContext.PersonalDetails.Find(data.UserId);
                    if (personalData == null)
                    {
                        return false;
                    }
                    personalData.FullName = data.FullName;
                    personalData.MobileNo = data.MobileNo;
                    personalData.Address = data.Address;
                    personalData.Email = data.Email;
                    personalData.Profile = data.Profile;
                    personalData.About = data.About;
                    personalData.Summary = data.Summary;
                    personalData.Modified_by = userId;
                    personalData.Modified_dt = DateTime.ParseExact(formattedDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    _dbContext.SaveChanges();
                }
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }
        public List<PersonalDtl> GetPersonalDtl(string token)
        {
            try
            {
                int userId = int.TryParse(StaticHelper.GetDetail(token, "Id"), out var parsedId) ? parsedId : 0;
                var data = _dbContext.PersonalDetails
                    .Where(s => s.Created_by == userId && s.Status == "E")
                    .Join(_dbContext.UserTbl,
                    s => s.Created_by, u => u.Id,
                    (s, u) => new PersonalDtl
                    {
                        UserId = s.UserId,
                        FullName = s.FullName,
                        MobileNo = s.MobileNo,
                        Email= s.Email,
                        Profile= s.Profile,
                        About=s.About,
                        Address=s.Address,
                        Summary=s.Summary,
                    })
                    .OrderByDescending(s => s.UserId)
                    .ToList();
                return data;
            }
            catch (Exception)
            {
                return new List<PersonalDtl>();
            }

        }
        public bool DeletePersonalData(int id, string token)
        {
            try
            {
                string formattedDate = DateTime.Now.ToString("dd-MMM-yyyy");
                int userId = int.TryParse(StaticHelper.GetDetail(token, "Id"), out var parsedId) ? parsedId : 0;
                var data = _dbContext.PersonalDetails.Find(id);
                if (data == null)
                {
                    return false;
                }
                data.Status = "D";
                data.Modified_by = userId;
                data.Modified_dt = DateTime.ParseExact(formattedDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public PersonalDtl GetPersonalDtById(int? id)
        {
            try
            {
                var data = _dbContext.PersonalDetails
                    .Where(s => s.UserId == id)
                    .Select(s => new PersonalDtl 
                    { UserId = s.UserId,
                      FullName = s.FullName,
                        MobileNo=s.MobileNo,
                        Email=s.Email,
                        About=s.About,
                        Summary=s.Summary,
                        Profile=s.Profile,
                        Address=s.Address })
                    .FirstOrDefault();

                return data;
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (logging, error handling, etc.)
                throw new Exception("Error fetching data by ID", ex);
            }
        }
        #endregion Personal Details
        #region Education
        public bool AddEducationData(EducationDtl data, string token)
        {
            try
            {
                string formattedDate = DateTime.Now.ToString("dd-MMM-yyyy");
                int userId = int.TryParse(StaticHelper.GetDetail(token, "Id"), out var parsedId) ? parsedId : 0;
                if (data.Id == 0)
                {
                    int maxId = GetMaxId<EducationDetail>("Id");
                    var newData = new EducationDetail()
                    {
                        Id = maxId,
                        Institution = data.Institution,
                        Location = data.Location,
                        Degree = data.Degree,
                        Grade = data.Grade,
                        FieldStudy = data.FieldStudy,
                        Specialization = data.Specialization,
                        StartDt = data.StartDt.HasValue
                          ? DateTime.ParseExact(data.StartDt.Value.ToString("dd-MMM-yyyy"),
                                                "dd-MMM-yyyy",
                                                System.Globalization.CultureInfo.InvariantCulture)
                          : (DateTime?)null,
                        EndDt = data.EndDt.HasValue
                        ? DateTime.ParseExact(data.EndDt.Value.ToString("dd-MMM-yyyy"),
                                              "dd-MMM-yyyy",
                                              System.Globalization.CultureInfo.InvariantCulture)
                        : (DateTime?)null,
                        Summary = data.Summary,
                        Status = "E",
                        Created_by = userId,
                        Created_dt = DateTime.ParseExact(formattedDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    };
                    _dbContext.EducationDetail.Add(newData);
                    _dbContext.SaveChanges();
                }
                else
                {
                    var educationDtl = _dbContext.EducationDetail.Find(data.Id);
                    if (educationDtl == null)
                    {
                        return false;
                    }
                    educationDtl.Institution = data.Institution;
                    educationDtl.Location = data.Location;
                    educationDtl.Degree = data.Degree;
                    educationDtl.Grade = data.Grade;
                    educationDtl.FieldStudy = data.FieldStudy;
                    educationDtl.Specialization = data.Specialization;
                    educationDtl.StartDt = data.StartDt.HasValue
                                          ? DateTime.ParseExact(data.StartDt.Value.ToString("dd-MMM-yyyy"),
                                                                "dd-MMM-yyyy",
                                                                System.Globalization.CultureInfo.InvariantCulture)
                                          : (DateTime?)null;
                    educationDtl.EndDt = data.EndDt.HasValue
                        ? DateTime.ParseExact(data.EndDt.Value.ToString("dd-MMM-yyyy"),
                                              "dd-MMM-yyyy",
                                              System.Globalization.CultureInfo.InvariantCulture)
                        : (DateTime?)null;
                    educationDtl.Summary = data.Summary;
                    educationDtl.Summary = data.Summary;
                    educationDtl.Modified_by = userId;
                    educationDtl.Modified_dt = DateTime.ParseExact(formattedDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    _dbContext.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<EducationDtl> GetEducationDtl(string token)
        {
            try
            {
                int userId = int.TryParse(StaticHelper.GetDetail(token, "Id"), out var parsedId) ? parsedId : 0;
                var data = _dbContext.EducationDetail
                    .Where(s => s.Created_by == userId && s.Status == "E")
                    .Join(_dbContext.UserTbl,
                    s => s.Created_by, u => u.Id,
                    (s, u) => new EducationDtl
                    {
                        Id = s.Id,
                        Institution = s.Institution,
                        Degree = s.Degree,
                        Location = s.Location,
                        Grade = s.Grade,
                        FieldStudy = s.FieldStudy,
                        Specialization = s.Specialization,
                        Summary = s.Summary,
                        StartDt=s.StartDt,
                        EndDt=s.EndDt,
                    })
                    .OrderByDescending(s => s.Id)
                    .ToList();
                return data;
            }
            catch (Exception)
            {
                return new List<EducationDtl>();
            }

        }
        public bool DeleteEducationData(int id, string token)
        {
            try
            {
                string formattedDate = DateTime.Now.ToString("dd-MMM-yyyy");
                int userId = int.TryParse(StaticHelper.GetDetail(token, "Id"), out var parsedId) ? parsedId : 0;
                var data = _dbContext.EducationDetail.Find(id);
                if (data == null)
                {
                    return false;
                }
                data.Status = "D";
                data.Modified_by = userId;
                data.Modified_dt = DateTime.ParseExact(formattedDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public EducationDtl GetEducationDtById(int? id)
        {
            try
            {
                var data = _dbContext.EducationDetail
                    .Where(s => s.Id == id)
                    .Select(s => new EducationDtl
                    {
                        Id = s.Id,
                        Institution = s.Institution,
                        Degree = s.Degree,
                        Location = s.Location,
                        Grade = s.Grade,
                        FieldStudy = s.FieldStudy,
                        Specialization = s.Specialization,
                        Summary = s.Summary,
                        StartDt = s.StartDt,
                        EndDt = s.EndDt,
                    })
                    .FirstOrDefault();

                return data;
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (logging, error handling, etc.)
                throw new Exception("Error fetching data by ID", ex);
            }
        }
        #endregion
        #region Experience
        public bool AddExperienceData(ExperienceDtl data, string token)
        {
            try
            {
                string formattedDate = DateTime.Now.ToString("dd-MMM-yyyy");
                int userId = int.TryParse(StaticHelper.GetDetail(token, "Id"), out var parsedId) ? parsedId : 0;
                if (data.Id == 0)
                {
                    int maxId = GetMaxId<ExperienceDetail>("Id");
                    var newData = new ExperienceDetail()
                    {
                        Id = maxId,
                        Position = data.Position,
                        Location = data.Location,
                        Company = data.Company,
                        Skills = data.Skills,
                        Achievement = data.Achievement,
                        JobDescription = data.JobDescription,
                        StartDt = data.StartDt.HasValue
                          ? DateTime.ParseExact(data.StartDt.Value.ToString("dd-MMM-yyyy"),
                                                "dd-MMM-yyyy",
                                                System.Globalization.CultureInfo.InvariantCulture)
                          : (DateTime?)null,
                        EndDt = data.EndDt.HasValue
                        ? DateTime.ParseExact(data.EndDt.Value.ToString("dd-MMM-yyyy"),
                                              "dd-MMM-yyyy",
                                              System.Globalization.CultureInfo.InvariantCulture)
                        : (DateTime?)null,
                        Status = "E",
                        Created_by = userId,
                        Created_dt = DateTime.ParseExact(formattedDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture)
                    };
                    _dbContext.ExperienceDetail.Add(newData);
                    _dbContext.SaveChanges();
                }
                else
                {
                    var Dtl = _dbContext.ExperienceDetail.Find(data.Id);
                    if (Dtl == null)
                    {
                        return false;
                    }
                    Dtl.Position = data.Position;
                    Dtl.Location = data.Location;
                    Dtl.Company = data.Company;
                    Dtl.Skills = data.Skills;
                    Dtl.Achievement = data.Achievement;
                    Dtl.JobDescription = data.JobDescription;
                    Dtl.StartDt = data.StartDt.HasValue
                      ? DateTime.ParseExact(data.StartDt.Value.ToString("dd-MMM-yyyy"),
                                            "dd-MMM-yyyy",
                                            System.Globalization.CultureInfo.InvariantCulture)
                      : (DateTime?)null;
                    Dtl.EndDt = data.EndDt.HasValue
                    ? DateTime.ParseExact(data.EndDt.Value.ToString("dd-MMM-yyyy"),
                                          "dd-MMM-yyyy",
                                          System.Globalization.CultureInfo.InvariantCulture)
                    : (DateTime?)null;
                    Dtl.Modified_by = userId;
                    Dtl.Modified_dt = DateTime.ParseExact(formattedDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    _dbContext.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<ExperienceDtl> GetExperienceDtl(string token)
        {
            try
            {
                int userId = int.TryParse(StaticHelper.GetDetail(token, "Id"), out var parsedId) ? parsedId : 0;
                var data = _dbContext.ExperienceDetail
                    .Where(s => s.Created_by == userId && s.Status == "E")
                    .Join(_dbContext.UserTbl,
                    s => s.Created_by, u => u.Id,
                    (s, u) => new ExperienceDtl
                    {
                        Id = s.Id,
                        Company = s.Company,
                        Position = s.Position,
                        Location = s.Location,
                        Skills = s.Skills,
                        JobDescription = s.JobDescription,
                        Achievement = s.Achievement,
                        StartDt = s.StartDt,
                        EndDt = s.EndDt,
                    })
                    .OrderByDescending(s => s.Id)
                    .ToList();
                return data;
            }
            catch (Exception)
            {
                return new List<ExperienceDtl>();
            }

        }
        public bool DeleteExperienceData(int id, string token)
        {
            try
            {
                string formattedDate = DateTime.Now.ToString("dd-MMM-yyyy");
                int userId = int.TryParse(StaticHelper.GetDetail(token, "Id"), out var parsedId) ? parsedId : 0;
                var data = _dbContext.ExperienceDetail.Find(id);
                if (data == null)
                {
                    return false;
                }
                data.Status = "D";
                data.Modified_by = userId;
                data.Modified_dt = DateTime.ParseExact(formattedDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public ExperienceDtl GetExperienceDtById(int? id)
        {
            try
            {
                var data = _dbContext.ExperienceDetail
                    .Where(s => s.Id == id)
                    .Select(s => new ExperienceDtl
                    {
                        Id = s.Id,
                        Company = s.Company,
                        Position = s.Position,
                        Location = s.Location,
                        Skills = s.Skills,
                        JobDescription = s.JobDescription,
                        Achievement = s.Achievement,
                        StartDt = s.StartDt,
                        EndDt = s.EndDt,
                    })
                    .FirstOrDefault();

                return data;
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (logging, error handling, etc.)
                throw new Exception("Error fetching data by ID", ex);
            }
        }
        #endregion
        public int GetMaxId<T>(string columnName) where T : class
        {
            try
            {
                var maxId = _dbContext.Set<T>()
                    .Max(e => EF.Property<int?>(e, columnName)) ?? 0;

                return maxId + 1;
            }
            catch
            {
                return 1; // Default to 1 if an error occurs
            }
        }

    }
}
