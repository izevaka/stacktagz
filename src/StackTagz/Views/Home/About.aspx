<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">Stack Tagz - About</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>About Stack Tagz</h2>
    <div class="normaltext">
        <p>
            Stack Tagz graphs user participation in Stack Exchange communities by looking at
            which tags a user is involved in. Every question, answer or comment counts as one
            point. The results are summarized by week. This means that every point on the X-Axis
            is one week.
        </p>
        <p>
            Stack Tagz is developed and maintained by Igor Zevaka, who blogs about development
            <a href="http://www.somethingorothersoft.com">here</a>. For general discussion about
            this site, refer to <a href="http://stackapps.com/questions/933/stacktagz-track-the-topics-you-are-interested-in-over-time">
                this StackApps question</a>.</a>
        </p>
    </div>
    <h2>Technologies that made this site possible</h2>
    <div class="normaltext">
    <table>
    <tr><td><a href="http://www.asp.net/mvc">ASP .NET MVC</a></td><td>MVC framework for ASP .NET</td></tr>
    <tr><td><a href="http://stacky.codeplex.com/">Stacky</a></td><td>Awesome .NET StackExchange API library</td></tr>
    <tr><td><a href="http://jquery.com"><img width="200" src="<%= Url.Content("~/Content/jquery_logo_color_onwhite.png") %>" alt="jquery logo"/></a></td><td>Lightweight JavaScript framework.</td></tr>
    <tr><td><a href="http://jqqueryui.com"><img width="200" src="<%= Url.Content("~/Content/JQuery_UI_Logo.png") %>" alt="jqueryui logo"/></a></td><td>Jquery User Interface Library.</td></tr>
    <tr><td><a href="http://jqplot.com"><img width="200" src="<%= Url.Content("~/Content/jqplot-logo.jpg") %>" alt="jqplot logo"/></a></td><td>Jquery plugin that provides charting functionality.</td></tr>
    <tr><td><a href="http://jquery.malsup.com/block/">jQuery BlockUI Plugin</a></td><td>Easy to use jQuery plugin that allows to mask elements, e.g. while data is loading.</td></tr>
    <tr><td><a href="http://www.asual.com/jquery/address/">jQuery Address</a></td><td>Comprehensive jQury plugin for handling havigation within one page (i.e. AJAX back button support).</td></tr>
    <tr><td><a href="http://www.codethinked.com/post/2010/05/26/SquishIt-The-Friendly-ASPNET-JavaScript-and-CSS-Squisher.aspx">SquishIt</a></td><td>Awesome JS and CSS minifier for ASP .NET</td></tr>

    </table>
    </div>
</asp:Content>
