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
        public void Init<TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, int defaultPage = 1, int maxSize = 10)
        {

            if (keySelector != null)
            {
                Source = source?.OrderBy(keySelector).ToList();

            }
            else
            {
                Source = source?.ToList();
            }
            Source = Source ?? new List<TSource>();
            MaxSize = maxSize;
            Total = Source.Count;
            TotalPage = Convert.ToInt32(Math.Ceiling(Total * 1.0 / MaxSize));
            if (TotalPage > 0)
            {
                if (defaultPage == 1)
                {
                    FirstPage();

                }
                else
                {
                    LinkPage(defaultPage);

                }

            }
        }


        public bool NextPage()
        {
            if (CurrentPage >= TotalPage)
            {
                return false;
            }
            CurrentPage++;
            int maxNum = CurrentPage < TotalPage ? MaxSize : Total - (CurrentPage - 1) * MaxSize;

            var list = Source.Skip((CurrentPage - 1) * MaxSize).Take(maxNum);

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
            int maxNum = Total >= MaxSize ? MaxSize : Total;
            var list = Source?.Take(maxNum);
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

        public void LinkPage(int pageIndex)
        {
            if (pageIndex > TotalPage)
            {
                pageIndex = TotalPage;
            }
            CurrentPage = pageIndex;
            int maxNum = CurrentPage < TotalPage ? MaxSize : Total - (CurrentPage - 1) * MaxSize;
            var list = Source.Skip((CurrentPage - 1) * MaxSize).Take(maxNum);
            PagePagingEvent?.Invoke(list, CurrentPage, TotalPage);

        }
    }
}
