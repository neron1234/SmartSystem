using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.ViewModel
{
    public class PagingModel<TSource> where TSource : new()
    {
        List<TSource> Source;
        private int MaxSize;

        public int TotalPage { private set; get; }
        public int CurrentPage { private set; get; } = 1;
        private int Total = 0;

        public event Action<IEnumerable<TSource>, int, int> PagePagingEvent;
        public void Init<TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, int maxSize = 10)
        {

            Source = source.OrderBy(keySelector).ToList();
            MaxSize = maxSize;
            Total = Source.Count;
            TotalPage = Convert.ToInt32(Math.Ceiling(Total * 1.0 / MaxSize));
            FirstPage();
        }

        public bool NextPage()
        {
            if (CurrentPage >= TotalPage)
            {
                return false;
            }
            int maxNum = Total >= (CurrentPage + 1) * MaxSize ? MaxSize : Total - (CurrentPage * MaxSize);
            var list = Source.Skip(CurrentPage * MaxSize).Take(maxNum);
            CurrentPage++;

            PagePagingEvent?.Invoke(list, CurrentPage, TotalPage);
            return true;
        }

        public bool PrePage()
        {
            if (CurrentPage <= 1)
            {
                return false;
            }

            var list = Source.Skip((CurrentPage - 1) * MaxSize).Take(MaxSize);
            CurrentPage--;

            PagePagingEvent?.Invoke(list, CurrentPage, TotalPage);
            return true;
        }

        public void FirstPage()
        {
            CurrentPage = 1;
            int maxNum = Total >= CurrentPage * MaxSize ? MaxSize : Total - (CurrentPage * MaxSize);
            var list = Source.Take(maxNum);
            PagePagingEvent?.Invoke(list, CurrentPage, TotalPage);
        }

        public void LastPage()
        {
            CurrentPage = TotalPage - 1;
            NextPage();
        }

        public void CyclePage()
        {
            if (!NextPage())
            {
                FirstPage();
            }

        }
    }
}
