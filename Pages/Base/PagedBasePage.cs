using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ApiRechargement.Web.Pages.Base
{
    public abstract class PagedBasePage : BasePage
    {
        protected int _currentPage = 1;
        protected string _columnSort;
        protected ListSortDirection _SortDirection = ListSortDirection.Descending;

        public abstract Task SortByColumnAction();

        public async Task SortByColumn(string columnName)
        {
            if (columnName == _columnSort)
            {
                _SortDirection = _SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            }

            _columnSort = columnName;
            await SortByColumnAction();
        }

        public string SetSortIcon(string columnName)
        {
            if (_columnSort == columnName)
            {
                if (_SortDirection == ListSortDirection.Ascending)
                {
                    return "fa-sort-up";
                }
                else
                {
                    return "fa-sort-down";
                }
            }

            return string.Empty;
        }
    }
}
