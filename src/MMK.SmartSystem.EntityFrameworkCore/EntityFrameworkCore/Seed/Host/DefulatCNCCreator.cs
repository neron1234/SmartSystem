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

            string[] kindTypes = { "", "大轮廓", "中轮廓", "小轮廓", "正常切割", "快速切割", "慢速切割", "打标", "其他", "其他", "其他" };

            for (int i = 1; i <= 10; i++)
            {
                var kind = _context.MachiningKinds.IgnoreQueryFilters().FirstOrDefault(d => d.Code == i);
                if (kind == null)
                {
                    _context.MachiningKinds.Add(new CNC.Core.LaserLibrary.MachiningKind()
                    {
                        Code = (short)i,
                        Description = "初始化创建",
                        Name_CN = kindTypes[i],
                        Name_EN = kindTypes[i]
                    });
                }
            }


            string[] nozzleTypes = { "", "单层割嘴", "双层割嘴" };

            for (int i = 1; i <= 2; i++)
            {
                var kind = _context.NozzleKinds.IgnoreQueryFilters().FirstOrDefault(d => d.Code == i);
                if (kind == null)
                {
                    _context.NozzleKinds.Add(new CNC.Core.LaserLibrary.NozzleKind()
                    {
                        Code = (short)i,
                        Name_CN = nozzleTypes[i],
                        Name_EN = nozzleTypes[i]
                    });
                }
            }


            string[] matrialTypes = { "", "碳钢", "不锈钢", "贴膜不锈钢", "铝", "黄铜", "紫铜", "镀锌板", "覆铝锌", "自定义", "其他" };

            for (int i = 1; i <= 10; i++)
            {
                var kind = _context.Materials.IgnoreQueryFilters().FirstOrDefault(d => d.Code == i);
                if (kind == null)
                {
                    _context.Materials.Add(new CNC.Core.LaserLibrary.Material()
                    {
                        Code = (short)i,
                        Name_CN = matrialTypes[i],
                        Name_EN = matrialTypes[i],
                        Description = "初始化创建"
                    });
                }
            }
        }
    }
}
