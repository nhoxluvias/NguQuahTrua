﻿using Data.DAL;
using Data.DTO;
using MSSQL_Lite.Access;
using MSSQL_Lite.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class CategoryBLL : BusinessLogicLayer
    {
        private DataAccessLevel dataAccessLevel;
        private bool disposed;

        public CategoryBLL(DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL();
            this.dataAccessLevel = dataAccessLevel;
            disposed = false;
        }

        public CategoryBLL(BusinessLogicLayer bll, DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL(bll.db);
            this.dataAccessLevel = dataAccessLevel;
            disposed = false;
        }

        private CategoryInfo ToCategoryInfo(Category category)
        {
            if (category == null)
                return null;
            return new CategoryInfo
            {
                ID = category.ID,
                name = category.name,
                description = category.description,
                createAt = category.createAt,
                updateAt = category.updateAt
            };
        }

        private Category ToCategory(CategoryCreation categoryCreation)
        {
            if (categoryCreation == null)
                throw new Exception("@'categoryCreation' must be not null");
            return new Category
            {
                name = categoryCreation.name,
                description = categoryCreation.description,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        private Category ToCategory(CategoryUpdate categoryUpdate)
        {
            if (categoryUpdate == null)
                throw new Exception("");
            return new Category
            {
                ID = categoryUpdate.ID,
                name = categoryUpdate.name,
                description = categoryUpdate.description,
                updateAt = DateTime.Now
            };
        }

        public async Task<List<CategoryInfo>> GetCategoriesAsync()
        {
            List<CategoryInfo> categories = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                categories = (await db.Categories.ToListAsync())
                    .Select(c => ToCategoryInfo(c)).ToList();
            else
                categories = (await db.Categories.ToListAsync(c => new { c.ID, c.name, c.description }))
                    .Select(c => ToCategoryInfo(c)).ToList();
            return categories;
        }

        public List<CategoryInfo> GetCategories()
        {
            List<CategoryInfo> categories = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                categories = db.Categories.ToList()
                    .Select(c => ToCategoryInfo(c)).ToList();
            else
                categories = db.Categories.ToList(c => new { c.ID, c.name, c.description })
                    .Select(c => ToCategoryInfo(c)).ToList();
            return categories;
        }

        public async Task<PagedList<CategoryInfo>> GetCategoriesAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<Category> pagedList = null;
            Expression<Func<Category, object>> orderBy = c => new { c.ID };
            if (dataAccessLevel == DataAccessLevel.Admin)
                pagedList = await db.Categories.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Categories.ToPagedListAsync(
                    c => new { c.ID, c.name, c.description },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<CategoryInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToCategoryInfo(c)).ToList()
            };
        }

        public PagedList<CategoryInfo> GetCategories(int pageIndex, int pageSize)
        {
            SqlPagedList<Category> pagedList = null;
            Expression<Func<Category, object>> orderBy = c => new { c.ID };
            if (dataAccessLevel == DataAccessLevel.Admin)
                pagedList = db.Categories.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Categories.ToPagedList(
                    c => new { c.ID, c.name, c.description },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<CategoryInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToCategoryInfo(c)).ToList()
            };
        }

        public async Task<CategoryInfo> GetCategoryAsync(int categoryId)
        {
            if (categoryId <= 0)
                throw new Exception("");
            Category category = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                category = await db.Categories
                     .SingleOrDefaultAsync(c => c.ID == categoryId);
            else
                category = await db.Categories
                    .SingleOrDefaultAsync(c => new { c.ID, c.name, c.description }, c => c.ID == categoryId);

            return ToCategoryInfo(category);
        }

        public CategoryInfo GetCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new Exception("");
            Category category = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                category = db.Categories
                     .SingleOrDefault(c => c.ID == categoryId);
            else
                category = db.Categories
                    .SingleOrDefault(c => new { c.ID, c.name, c.description }, c => c.ID == categoryId);

            return ToCategoryInfo(category);
        }

        public async Task<List<CategoryInfo>> GetCategoriesByFilmIdAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            if (dataAccessLevel == DataAccessLevel.Admin)
                sqlCommand.CommandText = @"Select [Category].* from [CategoryDistribution], [Category]
                                where [CategoryDistribution].[categoryID] = [Category].[ID]
                                    and [CategoryDistribution].[filmId] = @filmId";
            else
                sqlCommand.CommandText = @"Select [Category].[ID], [Category].[name], [Category].[description] 
                                from [CategoryDistribution], [Category]
                                where [CategoryDistribution].[categoryID] = [Category].[ID]
                                    and [CategoryDistribution].[filmId] = @filmId";

            sqlCommand.Parameters.Add(new SqlParameter("@filmId", filmId));
            return await db.ExecuteReaderAsync<List<CategoryInfo>>(sqlCommand);
        }

        public List<CategoryInfo> GetCategoriesByFilmId(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            if (dataAccessLevel == DataAccessLevel.Admin)
                sqlCommand.CommandText = @"Select [Category].* from [CategoryDistribution], [Category]
                                where [CategoryDistribution].[categoryID] = [Category].[ID]
                                    and [CategoryDistribution].[filmId] = @filmId";
            else
                sqlCommand.CommandText = @"Select [Category].[ID], [Category].[name], [Category].[description] 
                                from [CategoryDistribution], [Category]
                                where [CategoryDistribution].[categoryID] = [Category].[ID]
                                    and [CategoryDistribution].[filmId] = @filmId";

            sqlCommand.Parameters.Add(new SqlParameter("@filmId", filmId));
            return db.ExecuteReader<List<CategoryInfo>>(sqlCommand);
        }

        public async Task<StateOfCreation> CreateCategoryAsync(CategoryCreation categoryCreation)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            Category category = ToCategory(categoryCreation);
            if (category.name == null)
                throw new Exception("");

            int checkExists = (int)await db.Categories.CountAsync(c => c.name == category.name);
            if (checkExists != 0)
                return StateOfCreation.AlreadyExists;

            int affected;
            if (category.description == null)
                affected = await db.Categories.InsertAsync(category, new List<string> { "ID", "description" });
            else
                affected = await db.Categories.InsertAsync(category, new List<string> { "ID" });

            return (affected == 0) ? StateOfCreation.Failed : StateOfCreation.Success;
        }

        public async Task<StateOfUpdate> UpdateCategoryAsync(CategoryUpdate categoryUpdate)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            Category category = ToCategory(categoryUpdate);
            if (category.name == null)
                throw new Exception("");

            int affected;
            if (category.description == null)
                affected = await db.Categories.UpdateAsync(
                    category,
                    c => new { c.name, c.updateAt },
                    c => c.ID == category.ID
                );
            else
                affected = await db.Categories.UpdateAsync(
                    category,
                    c => new { c.name, c.description, c.updateAt },
                    c => c.ID == category.ID
                );

            return (affected == 0) ? StateOfUpdate.Failed : StateOfUpdate.Success;
        }

        public async Task<StateOfDeletion> DeleteCategoryAsync(int categoryId)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            if (categoryId <= 0)
                throw new Exception("");

            long categoryDistributionNumber = await db.CategoryDistributions
                .CountAsync(cd => cd.categoryId == categoryId);
            if (categoryDistributionNumber > 0)
                return StateOfDeletion.ConstraintExists;

            int affected = await db.Categories.DeleteAsync(c => c.ID == categoryId);
            return (affected == 0) ? StateOfDeletion.Failed : StateOfDeletion.Success;
        }

        public async Task<int> CountAllAsync()
        {
            return (int)await db.Categories.CountAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {

                    }
                    this.disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}
