﻿using System;
using System.Data.Entity;
using TTcms.Domain;
using TTcms.Infrastructure.Domain;
using TTcms.Infrastructure.Domain.Uow;

namespace TTcms.EFRepositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual void RegisterAdded(EntityBase entityBase, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _dbContext.Set(entityBase.GetType()).Add(entityBase);
        }

        public virtual void RegisterChangeded(EntityBase entityBase, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _dbContext.Entry(entityBase).State = EntityState.Modified;
        }

        public virtual void RegisterRemoved(EntityBase entityBase, IUnitOfWorkRepository unitOfWorkRepository)
        {
            _dbContext.Entry(entityBase).State = EntityState.Deleted;
        }

        public virtual void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
