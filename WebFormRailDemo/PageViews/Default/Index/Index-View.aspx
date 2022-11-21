<%@ Page Language="C#" MasterPageFile="~/PageViews/Default/MasterPageMain.master" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphFull" Runat="Server">
<h4>WebFormRail 是一个基于.net 2.0 的web framework，基于rail的一个MVC 开发框架，框架参考和借鉴了ROR,MonoRail,Spring.net,ProMesh,asp.net MVC等开源框架的优秀设计思想和技术。
<br />WebFormRail的视图引擎沿用了微软的asp.net视图引擎，并且能够完整的运用webform的所有控件。
<br />WebFormRail的设计宗旨是“约定胜过配置”，统一、规范的约定是保证团队的开发一致的基础，框架在保证统一的基础上为项目带来更高的开发效率。
</h4>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphLeft" Runat="Server">



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRight" Runat="Server">

<UC:DemoControl ID="ControllerList" runat="server" />
</asp:Content>

