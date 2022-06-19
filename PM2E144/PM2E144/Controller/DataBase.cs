using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using PM2E144.Models;
using System.Threading.Tasks;

namespace PM2E144.Controller
{
    public class DataBase
    {
        readonly SQLiteAsyncConnection dbase;

        public DataBase(string dbpath)
        {
            dbase = new SQLiteAsyncConnection(dbpath);
            dbase.CreateTableAsync<Sites>();
        }

        public Task<int> SaveSiteAsync(Sites site)
        {
            if(site.id != 0)
            {
                return dbase.UpdateAsync(site);
            }
            else
            {
                return dbase.InsertAsync(site);
            }
        }

        public Task<List<Sites>> GetSitesAsync()
        {
            return dbase.Table<Sites>().ToListAsync();
        }

        public Task<Sites> GetSitesByIdAsync(int idSite)
        {
            return dbase.Table<Sites>().Where(a => a.id == idSite).FirstOrDefaultAsync();
        }

        public Task<int> DeleteSiteAsync(Sites site)
        {
            return dbase.DeleteAsync(site);
        }
    }
}
