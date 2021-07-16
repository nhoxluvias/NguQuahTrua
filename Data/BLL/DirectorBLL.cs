﻿using Common.Web;
using Data.DAL;
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
    public class DirectorBLL : BusinessLogicLayer
    {
        private bool disposed;
        private bool includeDescription;

        public bool IncludeDescription { set { includeDescription = value; } }

        public DirectorBLL()
            : base()
        {
            InitDAL();
            SetDefault();
            disposed = false;
        }

        public DirectorBLL(BusinessLogicLayer bll)
            : base()
        {
            InitDAL(bll.db);
            SetDefault();
            disposed = false;
        }

        public override void SetDefault()
        {
            base.SetDefault();
            includeDescription = false;
        }

        private DirectorInfo ToDirectorInfo(Director director)
        {
            if (director == null)
                return null;

            DirectorInfo directorInfo = new DirectorInfo();
            directorInfo.ID = director.ID;
            directorInfo.name = director.name;

            if (includeDescription)
                directorInfo.description = director.description;

            if (includeTimestamp)
            {
                directorInfo.createAt = director.createAt;
                directorInfo.updateAt = director.updateAt;
            }

            return directorInfo;
        }

        private Director ToDirector(DirectorCreation directorCreation)
        {
            if (directorCreation == null)
                throw new Exception("@'directorCreation' must be not null");

            return new Director
            {
                name = directorCreation.name,
                description = directorCreation.description,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        private Director ToDirector(DirectorUpdate directorUpdate)
        {
            if (directorUpdate == null)
                throw new Exception("@'directorCreation' must be not null");

            return new Director
            {
                ID = directorUpdate.ID,
                name = directorUpdate.name,
                description = directorUpdate.description,
                updateAt = DateTime.Now
            };
        }

        public async Task<List<DirectorInfo>> GetDirectorsAsync()
        {
            List<DirectorInfo> directors = null;
            if (includeDescription && includeTimestamp)
                directors = (await db.Directors.ToListAsync()).Select(d => ToDirectorInfo(d)).ToList();
            else if (includeDescription)
                directors = (await db.Directors.ToListAsync(c => new { c.ID, c.name, c.description }))
                    .Select(d => ToDirectorInfo(d)).ToList();
            else if (includeTimestamp)
                directors = (await db.Directors.ToListAsync(c => new { c.ID, c.name, c.createAt, c.updateAt }))
                    .Select(d => ToDirectorInfo(d)).ToList();
            else
                directors = (await db.Directors.ToListAsync(c => new { c.ID, c.name }))
                    .Select(d => ToDirectorInfo(d)).ToList();

            return directors;
        }

        public List<DirectorInfo> GetDirectors()
        {
            List<DirectorInfo> directors = null;
            if (includeDescription && includeTimestamp)
                directors = db.Directors.ToList().Select(d => ToDirectorInfo(d)).ToList();
            else if (includeDescription)
                directors = db.Directors.ToList(c => new { c.ID, c.name, c.description })
                    .Select(d => ToDirectorInfo(d)).ToList();
            else if (includeTimestamp)
                directors = db.Directors.ToList(c => new { c.ID, c.name, c.createAt, c.updateAt })
                    .Select(d => ToDirectorInfo(d)).ToList();
            else
                directors = db.Directors.ToList(c => new { c.ID, c.name })
                    .Select(d => ToDirectorInfo(d)).ToList();

            return directors;
        }

        public PagedList<DirectorInfo> GetDirectors(int pageIndex, int pageSize)
        {
            SqlPagedList<Director> pagedList = null;
            Expression<Func<Director, object>> orderBy = c => new { c.ID };
            if (includeDescription && includeTimestamp)
                pagedList = db.Directors.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if (includeDescription)
                pagedList = db.Directors.ToPagedList(
                    c => new { c.ID, c.name, c.description }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if (includeTimestamp)
                pagedList = db.Directors.ToPagedList(
                    c => new { c.ID, c.name, c.createAt, c.updateAt }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Directors.ToPagedList(
                    c => new { c.ID, c.name }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);

            return new PagedList<DirectorInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToDirectorInfo(c)).ToList()
            };
        }

        public async Task<PagedList<DirectorInfo>> GetDirectorsAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<Director> pagedList = null;
            Expression<Func<Director, object>> orderBy = c => new { c.ID };
            if (includeDescription && includeTimestamp)
                pagedList = await db.Directors.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if(includeDescription)
                pagedList = await db.Directors.ToPagedListAsync(
                    c => new { c.ID, c.name, c.description }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if(includeTimestamp)
                pagedList = await db.Directors.ToPagedListAsync(
                    c => new { c.ID, c.name, c.createAt, c.updateAt }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Directors.ToPagedListAsync(
                    c => new { c.ID, c.name }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            
            return new PagedList<DirectorInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToDirectorInfo(c)).ToList()
            };
        }

        public async Task<DirectorInfo> GetDirectorAsync(long directorId)
        {
            if (directorId <= 0)
                throw new Exception("");

            Director director = null;
            if (includeDescription && includeTimestamp)
                director = await db.Directors.SingleOrDefaultAsync(d => d.ID == directorId);
            else if(includeDescription)
                director = await db.Directors
                    .SingleOrDefaultAsync(d => new { d.ID, d.name, d.description }, d => d.ID == directorId);
            else if(includeTimestamp)
                director = await db.Directors
                    .SingleOrDefaultAsync(d => new { d.ID, d.name, d.createAt, d.updateAt }, d => d.ID == directorId);
            else
                director = await db.Directors
                    .SingleOrDefaultAsync(d => new { d.ID, d.name }, d => d.ID == directorId);

            return ToDirectorInfo(director);
        }

        public DirectorInfo GetDirector(long directorId)
        {
            if (directorId <= 0)
                throw new Exception("");

            Director director = null;
            if (includeDescription && includeTimestamp)
                director = db.Directors.SingleOrDefault(d => d.ID == directorId);
            else if(includeDescription)
                director = db.Directors
                    .SingleOrDefault(d => new { d.ID, d.name, d.description }, d => d.ID == directorId);
            else if(includeTimestamp)
                director = db.Directors
                    .SingleOrDefault(d => new { d.ID, d.name, d.createAt, d.updateAt }, d => d.ID == directorId);
            else
                director = db.Directors
                    .SingleOrDefault(d => new { d.ID, d.name }, d => d.ID == directorId);

            return ToDirectorInfo(director);
        }

        public async Task<List<DirectorInfo>> GetDirectorsByFilmIdAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            if (includeDescription && includeTimestamp)
                sqlCommand.CommandText = @"Select [Director].* from [Director], [DirectorOfFilm] 
                            where [Director].[ID] = [DirectorOfFilm].[directorId]
                                and [DirectorOfFilm].[filmId] = @filmId";
            else if(includeDescription)
                sqlCommand.CommandText = @"Select [Director].[ID], [Director].[name], [Director].[description] 
                            from [Director], [DirectorOfFilm] 
                            where [Director].[ID] = [DirectorOfFilm].[directorId]
                                and [DirectorOfFilm].[filmId] = @filmId";
            else if (includeTimestamp)
                sqlCommand.CommandText = @"Select [Director].[ID], [Director].[name], [Director].[createAt], [Director].[updateAt] 
                            from [Director], [DirectorOfFilm] 
                            where [Director].[ID] = [DirectorOfFilm].[directorId]
                                and [DirectorOfFilm].[filmId] = @filmId";
            else
                sqlCommand.CommandText = @"Select [Director].[ID], [Director].[name]
                            from [Director], [DirectorOfFilm] 
                            where [Director].[ID] = [DirectorOfFilm].[directorId]
                                and [DirectorOfFilm].[filmId] = @filmId";

            sqlCommand.Parameters.Add(new SqlParameter("@filmId", filmId));
            return await db.ExecuteReaderAsync<List<DirectorInfo>>(sqlCommand);
        }

        public List<DirectorInfo> GetDirectorsByFilmId(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            if (includeDescription && includeTimestamp)
                sqlCommand.CommandText = @"Select [Director].* from [Director], [DirectorOfFilm] 
                            where [Director].[ID] = [DirectorOfFilm].[directorId]
                                and [DirectorOfFilm].[filmId] = @filmId";
            else if (includeDescription)
                sqlCommand.CommandText = @"Select [Director].[ID], [Director].[name], [Director].[description] 
                            from [Director], [DirectorOfFilm] 
                            where [Director].[ID] = [DirectorOfFilm].[directorId]
                                and [DirectorOfFilm].[filmId] = @filmId";
            else if (includeTimestamp)
                sqlCommand.CommandText = @"Select [Director].[ID], [Director].[name], [Director].[createAt], [Director].[updateAt] 
                            from [Director], [DirectorOfFilm] 
                            where [Director].[ID] = [DirectorOfFilm].[directorId]
                                and [DirectorOfFilm].[filmId] = @filmId";
            else
                sqlCommand.CommandText = @"Select [Director].[ID], [Director].[name]
                            from [Director], [DirectorOfFilm] 
                            where [Director].[ID] = [DirectorOfFilm].[directorId]
                                and [DirectorOfFilm].[filmId] = @filmId";

            sqlCommand.Parameters.Add(new SqlParameter("@filmId", filmId));
            return db.ExecuteReader<List<DirectorInfo>>(sqlCommand);
        }

        public async Task<StateOfCreation> CreateDirectorAsync(DirectorCreation directorCreation)
        {
            Director director = ToDirector(directorCreation);
            if (director.name == null)
                throw new Exception("");

            long checkExists = await db.Directors.CountAsync(c => c.name == director.name);
            if (checkExists != 0)
                return StateOfCreation.AlreadyExists;

            int affected;
            if (director.description == null)
                affected = await db.Directors.InsertAsync(director, new List<string> { "ID", "description" });
            else
                affected = await db.Directors.InsertAsync(director, new List<string> { "ID" });

            return (affected == 0) ? StateOfCreation.Failed : StateOfCreation.Success;
        }

        public async Task<StateOfUpdate> UpdateDirectorAsync(DirectorUpdate directorUpdate)
        {
            Director director = ToDirector(directorUpdate);
            if (director.name == null)
                throw new Exception("");

            int affected;
            if (director.description == null)
                affected = await db.Directors.UpdateAsync(
                    director,
                    d => new { d.name, d.updateAt },
                    d => d.ID == director.ID
                );
            else
                affected = await db.Directors.UpdateAsync(
                    director,
                    d => new { d.name, d.description, d.updateAt },
                    d => d.ID == director.ID
                );

            return (affected == 0) ? StateOfUpdate.Failed : StateOfUpdate.Success;
        }

        public async Task<StateOfDeletion> DeleteDirectorAsync(long directorId)
        {
            if (directorId <= 0)
                throw new Exception("");

            long directorOfFilmNumber = await db.DirectorOfFilms.CountAsync(df => df.directorId == directorId);
            if (directorOfFilmNumber > 0)
                return StateOfDeletion.ConstraintExists;

            int affected = await db.Directors.DeleteAsync(d => d.ID == directorId);
            return (affected == 0) ? StateOfDeletion.Failed : StateOfDeletion.Success;
        }

        public async Task<long> CountAllAsync()
        {
            return await db.Directors.CountAsync();
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
