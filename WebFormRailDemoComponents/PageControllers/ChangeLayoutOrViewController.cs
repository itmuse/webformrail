using System;
using System.Collections.Generic;
using System.Text;
using WebFormRail;

namespace WebFormRailDemoComponents
{
    /// <summary>
    /// 关于母版页（Layout）约定：
    /// 约定（1）：母版页文件约定统一放在主题的根目录下，如默认主题为Default文件夹，MasterPage.master母版页或者其他
    /// 新增加的母版页统一放都在Default文件夹的根目录下。
    /// 约定（2）：当控制器（如ChangeLayoutOrViewController类）需要改变默认的母版页时，在控制器类增加Layout属性，Layout属性只支持对类进行设置。
    /// Layout属性值为母版页的名称，名称约定为母版页文件的名称但不带文件后缀名。
    /// ======================================================================
    /// 关于模板（View）约定：
    /// 约定（1）：格式约定：PageViews/[主题，如Default]/[控制器，如ChangeLayoutOrView]/[模板名称，如Index]-View.aspx。控制器的默认方法名为：Index，对应的模板文件为：Index-View.aspx，
    /// 一个控制器中可以有多个方法（Action），如果没有改变控制器或者方法的模板，则统一使用默认的Index模板文件。
    /// 约定（2）：要改变模板，可以使用View属性，View属性支持对控制器（类）和方法的设置。方法的View属性设置会覆盖控制器（类）的View属性设置，如果控制器和方法都同时设置了View
    /// 的属性，则产生影响的是方法的View属性设置。
    /// 约定（3）：View的属性值为模板的文件名称，但不包含“-View.aspx”部分。如果需要跨控制器文件夹进行模板改变，View的属性值包含控制器文件夹路径便可，如：View("News/Index").
    /// </summary>
    [Layout("MasterPageChanged")]//改变默认的母板
    [View("ChangeView")]//改变默认的模板
    public class ChangeLayoutOrViewController : WebFormController
    {
        public void Index()
        {
            View.Title = "更改模板或视图示例演示，当前action为：Index";
        }
        [View("Index")]//修改回默认的Index模板，此属性设置会覆盖[View("ChangeView")]的设置
        public void ChangeToIndexView()
        {
            View.Title = "更改模板或视图示例演示，当前action为：ChangeToIndexView";
        }
        [View("News/Index")]//修改模板为News的controller的模板，此属性设置会覆盖[View("ChangeView")]的设置
        public void ChangeToNewsView()
        {
            View.Title = "更改模板或视图示例演示，当期的action为：ChangeToNewsView";

            ViewData["Date"] = DateTime.Now;
        }
        [View("ChangeView")]//修改模板为ChangeView的模板，此属性设置会覆盖[View("ChangeView")]的设置
        public void ChangeView()
        {
            View.Title = "更改模板或视图示例演示，当前的action为：ChangeView";
        }
    }
}
