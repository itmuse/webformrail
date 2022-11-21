<%@ Page Language="C#" MasterPageFile="~/PageViews/Default/MasterPage.master" Title="无标题页" %>

<script runat="server">

</script>

<asp:Content ID="Content1" ContentPlaceHolderID="masterPage" Runat="Server">
<h5>
关于母版页（Layout）约定：<br />
 约定（1）：母版页文件约定统一放在主题的根目录下，如默认主题为Default文件夹，MasterPage.master母版页或者其他<br />
新增加的母版页统一放都在Default文件夹的根目录下。<br />
约定（2）：当控制器（如ChangeLayoutOrViewController类）需要改变默认的母版页时，在控制器类增加Layout属性，Layout属性只支持对类进行设置。<br />
Layout属性值为母版页的名称，名称约定为母版页文件的名称但不带文件后缀名。<br />
 ======================================================================<br />
关于模板（View）约定：<br />
约定（1）：格式约定：PageViews/[主题，如Default]/[控制器，如ChangeLayoutOrView]/[模板名称，如Index]-View.aspx。控制器的默认方法名为：Index，对应的模板文件为：Index-View.aspx，<br />
一个控制器中可以有多个方法（Action），如果没有改变控制器或者方法的模板，则统一使用默认的Index模板文件。<br />
约定（2）：要改变模板，可以使用View属性，View属性支持对控制器（类）和方法的设置。方法的View属性设置会覆盖控制器（类）的View属性设置，如果控制器和方法都同时设置了View<br />
的属性，则产生影响的是方法的View属性设置。<br />
约定（3）：View的属性值为模板的文件名称，但不包含“-View.aspx”部分。如果需要跨控制器文件夹进行模板改变，View的属性值包含控制器文件夹路径便可，如：View("News/Index").<br />
</h5>
<h3>默认的Action的View为“Index-View.aspx”，现已通过[View("ChangeView")]改变为：ChangeView-View.aspx</h3>
</asp:Content>

