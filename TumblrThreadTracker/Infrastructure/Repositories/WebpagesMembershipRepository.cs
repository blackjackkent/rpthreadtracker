using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models.DomainModels.Account;

namespace TumblrThreadTracker.Infrastructure.Repositories
{
    public class WebpagesMembershipRepository : IRepository<WebpagesMembership>
    {
        private readonly IThreadTrackerContext _context;

        public WebpagesMembershipRepository(IThreadTrackerContext context)
        {
            _context = context;
        }

        public WebpagesMembership Get(int id)
        {
            return _context.WebpagesMembership.FirstOrDefault(b => b.UserId == id);
        }

        public IEnumerable<WebpagesMembership> Get(Expression<Func<WebpagesMembership, bool>> criteria)
        {
            return _context.WebpagesMembership.Where(criteria);
        }

        public void Insert(WebpagesMembership entity)
        {
            _context.WebpagesMembership.Add(entity);
            _context.Commit();
        }

        public void Update(WebpagesMembership entity)
        {
            var toUpdate = _context.WebpagesMembership.FirstOrDefault(b => b.UserId == entity.UserId);
            if (toUpdate != null)
            {
                toUpdate.ConfirmationToken = entity.ConfirmationToken;
                toUpdate.CreateDate = entity.CreateDate;
                toUpdate.IsConfirmed = entity.IsConfirmed;
                toUpdate.LastPasswordFailureDate = entity.LastPasswordFailureDate;
                toUpdate.Password = entity.Password;
                toUpdate.PasswordChangedDate = entity.PasswordChangedDate;
                toUpdate.PasswordFailuresSinceLastSuccess = entity.PasswordFailuresSinceLastSuccess;
                toUpdate.PasswordSalt = entity.PasswordSalt;
                toUpdate.PasswordVerificationToken = entity.PasswordVerificationToken;
                toUpdate.PasswordVerificationTokenExpirationDate = entity.PasswordVerificationTokenExpirationDate;
                toUpdate.UserId = entity.UserId;
            }
            _context.Commit();
        }

        public void Delete(int? id)
        {
            var toUpdate = _context.WebpagesMembership.FirstOrDefault(b => b.UserId == id);
            _context.GetDbSet(typeof (WebpagesMembership)).Remove(toUpdate);
            _context.Commit();
        }
    }
}