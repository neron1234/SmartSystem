using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.EntityFrameworkCore.Seed.Host
{
    public class DefulatCNCCreator
    {
        private readonly SmartSystemDbContext _context;

        public DefulatCNCCreator(SmartSystemDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            #region GAS 空气初始化

            string[] arrTypes = { "", "空气", "氧气", "氮气" };
            for (int i = 1; i <= 3; i++)
            {
                var gas = _context.Gass.IgnoreQueryFilters().FirstOrDefault(u => u.Code == i);
                if (gas == null)
                {

                    _context.Gass.Add(new CNC.Core.LaserLibrary.Gas()
                    {
                        Code = (short)i,
                        Description = "初始化创建",
                        Name_CN = arrTypes[i],
                        Name_EN = arrTypes[i]
                    });

                }
            }

            #endregion

            //_context.MachiningKinds.IgnoreQueryFilters().FirstOrDefault(d=>d.)
        }
    }
}
