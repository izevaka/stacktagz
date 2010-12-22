<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StackTagz.Model.MainViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Stack Tagz
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        Plot user activity from a range of StackExchange sites
    </p>
    <div id="userinput">
        <div class="sitedetails">
            <div>
                <img class="siteimg" />
            </div>
            <label for="selectsite">
                Site:</label><input type="text" id="selectsite"/>
        </div>
        <div class="mainuserdetails">
            <div>
                <span class="userinfo">
                    <img class="userimg" />
                    <span class="username"></span>
                    <span class="rep"></span>
                </span>
            </div>
            
            <label for="selectuser">User:</label><input type="text" id="selectuser"/>
            <button id="getgraph" type="submit">Get History</button>
        </div>
        <div class="clear"></div>
    </div>
    <div id="graphandstack">
        <div id="graph" ></div>
        <div id="tagstack">
            <div id="stackcaption" class="animated">Your tagz stack (top 10)</div>
            <div id="tagstackinner" class="animated">
                <div class="level4">
                    
                </div>
                <div class="level3">
                    
                </div>
                <div class="level2">
                    
                </div>
                <div class="level1">
                    
                </div>
            </div>
        </div>
        <div id="summary">

        </div>
    </div>
    <script type="text/javascript">
        var siteData = <%= Newtonsoft.Json.JsonConvert.SerializeObject(Model.SiteInfoList, new [] { new StackTagz.Converters.SafeStringConverter() })%>;
        var dataUrl = "<%= Url.Content("~/data/graph")%>";
        var searchUsersUrl = "<%= Url.Content("~/data/users")%>";
        var userData;
        var siteSource = [];
        $.each(siteData, function(index,item){
            siteSource.push(item.Site);
        });
        var apiKey = "<%= Model.ApiKey %>";
        var apiVersion = "<%= Model.ApiVersion %>";
    </script>
</asp:Content>
