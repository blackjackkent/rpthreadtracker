using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using TumblrThreadTracker.Infrastructure.Entities;
using TumblrThreadTracker.Interfaces;
using TumblrThreadTracker.Models;
using TumblrThreadTracker.Models.DomainModels;

namespace TumblrThreadTracker.Infrastructure
{
    public static class ExtensionMethods
    {
        public static IQueryable<TModel> ToSelection<TModel, TEntity>(this IQueryable<TEntity> entities)
            where TModel : DomainModel
            where TEntity : IEntity
        {
            return entities.Project().To<TModel>();
        }

        public static TEntity ToEntity<TModel, TEntity>(this TModel model)
            where TModel : Model
            where TEntity : class, IEntity, new()
        {
            var entity = new TEntity();
            return Load(entity, model);
        }

        public static TEntity Load<TModel, TEntity>(this TEntity entity, TModel model)
            where TModel : Model
            where TEntity : class, IEntity, new()
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            if (model == null)
                throw new ArgumentNullException("model");
            return Mapper.Map(model, entity);
        }

        public static TModel ToModel<TModel, TEntity>(this TEntity entity)
            where TModel : DomainModel
            where TEntity : IEntity
        {
            return Mapper.Map<TModel>(entity);
        }
    }
}