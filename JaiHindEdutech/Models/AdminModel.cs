using JaiHindEdutech.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JaiHindEdutech.Models
{
    public class AdminModel
    {
        //private JaiHindEduEntitiesNew db = new JaiHindEduEntitiesNew();

        public int CreateCoachingCentre(CoachingCentreModel CoachingCentreModel)
        {
            try
            {
                int CoachingCentreId = 0;

                if (CoachingCentreModel.IsActive == null)
                    CoachingCentreModel.IsActive = true;
                using (var db = new JaiHindEduEntitiesNew())
                {
                    var coaching = db.CoachingCentres.Where(s => s.CoachingCentreId == CoachingCentreModel.CoachingCentreId).FirstOrDefault();
                    if (coaching != null && coaching.CoachingCentreId > 0)
                    {
                        coaching.CoachingCentreName = CoachingCentreModel.CoachingCentreName;
                        coaching.AddressLine1 = CoachingCentreModel.AddressLine1;
                        coaching.AddressLine2 = CoachingCentreModel.AddressLine2;
                        coaching.City = CoachingCentreModel.City;
                        coaching.State = CoachingCentreModel.State;
                        coaching.Zip = CoachingCentreModel.Zip;
                        coaching.ContactNo1 = CoachingCentreModel.ContactNo1;
                        coaching.IsActive = CoachingCentreModel.IsActive;
                        coaching.ModifiedBy = CoachingCentreModel.ModifiedBy;
                        coaching.ModifiedOn = CoachingCentreModel.ModifiedOn;

                        db.SaveChanges();
                        CoachingCentreId = coaching.CoachingCentreId;
                    }
                    else
                    {
                        var coachinginfo = new CoachingCentre
                        {
                            CoachingCentreName = CoachingCentreModel.CoachingCentreName,
                            AddressLine1 = CoachingCentreModel.AddressLine1,
                            AddressLine2 = CoachingCentreModel.AddressLine2,
                            City = CoachingCentreModel.City,
                            State = CoachingCentreModel.State,
                            Zip = CoachingCentreModel.Zip,
                            ContactNo1 = CoachingCentreModel.ContactNo1,
                            IsActive = CoachingCentreModel.IsActive,
                            CreatedBy = CoachingCentreModel.CreatedBy,
                            CreatedOn = CoachingCentreModel.CreatedOn,
                            ModifiedBy = CoachingCentreModel.ModifiedBy,
                            ModifiedOn = CoachingCentreModel.ModifiedOn
                        };
                        db.CoachingCentres.Add(coachinginfo);
                        db.SaveChanges();
                        CoachingCentreId = coachinginfo.CoachingCentreId;
                    }
                    return CoachingCentreId;
                }
            }
            catch
            {
                return 0;
            }
        }
        public CoachingCentreModel GetCoachingCentreInfo(int CoachingCentreId)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    return db.CoachingCentres.Where(c => c.IsActive == true && c.CoachingCentreId == CoachingCentreId).Select(c => new CoachingCentreModel
                    {
                        CoachingCentreId = c.CoachingCentreId,
                        CoachingCentreName = c.CoachingCentreName,
                        AddressLine1 = c.AddressLine1,
                        AddressLine2 = c.AddressLine2,
                        City = c.City,
                        State = c.State,
                        Zip = c.Zip,
                        ContactNo1 = c.ContactNo1,
                        IsActive = c.IsActive,
                        CreatedBy = c.CreatedBy,
                        CreatedOn = c.CreatedOn,
                        ModifiedBy = c.ModifiedBy,
                        ModifiedOn = c.ModifiedOn
                    }).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public CollegeInfoModel GetColelgeInfo(int CollegeInfoId)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    return db.CollegeInfoes.Where(c => c.IsActive == true && c.CollegeInfoId == CollegeInfoId).Select(c => new CollegeInfoModel
                    {
                        CollegeInfoId = c.CollegeInfoId,
                        CollegeName = c.CollegeName,
                        CollegeNameShort = c.CollegeNameShort,
                        CollegeType = c.CollegeType,
                        AddressLine1 = c.AddressLine1,
                        AddressLine2 = c.AddressLine2,
                        City = c.City,
                        State = c.State,
                        Zip = c.Zip,
                        ContactNo1 = c.ContactNo1,
                        ContactNo2 = c.ContactNo2,
                        LogoURL = c.LogoURL,
                        IsActive = c.IsActive
                    }).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}