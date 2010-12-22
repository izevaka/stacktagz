/// <reference path="JSLinq/scripts/JSLINQ-vsdoc.js" />
/// <reference path="jquery.address-1.2.1/jquery.address-1.2.1.js" />

$(function () {
    $("#menu a").button();
});

if (!window.console) {
    window.console = {};
    window.console.log = function () { };
}

function createTags(series, labels) {
    var level1 = $("#tagstackinner .level1")
    var level2 = $("#tagstackinner .level2")
    var level3 = $("#tagstackinner .level3")
    var level4 = $("#tagstackinner .level4")

    var sortedByLength = series.sort(function (a, b) { return a.label.length <= b.label.length });
    $.each(sortedByLength, function (index, value) {

        function appendTag(appendTo) {
            appendTo.append($("<span>" + value.label + "</span>").css("background-color", value.color));
        }

        if (index >= 0 && index < 4)
            appendTag(level1);
        if (index >= 4 && index < 7)
            appendTag(level2);
        if (index >= 7 && index < 9)
            appendTag(level3);
        if (index >= 9 && index < 10)
            appendTag(level4);
    });

    $("#tagstack .animated").fadeIn("slow");
}

function formatDataUrl(apiAddress, user) {
    return dataUrl + "/" + apiAddress + "/" + user;
}


function formatRep(num) {
	if(num >= 100000)
		return parseInt(num / 1000) + "k";
	if (num >= 10000) {
	    var rep = Math.floor(num / 100);
	    if (rep%10)
	        rep = (rep/10).toFixed(1);
	    else
	        rep = (rep/10).toFixed(0);
	    return rep + "k";
	}
return addCommas(num);
}

//taken from http://www.mredkj.com/javascript/numberFormat.html
function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}
function updateUserFromId(site, userId) {
    var apiAddress = findApiAddress(site);
    if (apiAddress) {
        $.ajax({
            url: "http://" + apiAddress + "/" + apiVersion + "/users/" + userId + "?key=" + apiKey,
            dataType: "jsonp",
            jsonp: "jsonp",
            success: function (data, status, xhr) {
                //TODO error handling
                if (data.users && data.users.length) {
                    var userInfo = apiUserToUserInfo(data.users[0]);
                    updateUserDisplay(userInfo);
                    window.userData = [userInfo];
                    selectUser.val(userInfo.Name);
                } else {
                    //put up block ui
                }
            }
        });
    }
}

function updateUserDisplay(userInfo) {
    if ($("#userinput").get(0).userInfo != userInfo) {
        if (userInfo) {
            $("#userinput .mainuserdetails .userimg").attr("src", userInfo.GravatarUrl).load(function () { $(".mainuserdetails div div,.mainuserdetails div span").fadeIn("slow"); });
            $("#userinput .mainuserdetails .username").text(userInfo.Name);
            $("#userinput .mainuserdetails .rep").text(userInfo.RepString);

            $("#userinput .userinfo .badge").remove();
            createBadge(userInfo.GoldBadges, "gold");
            createBadge(userInfo.SilverBadges, "silver");
            createBadge(userInfo.BronzeBadges, "bronze");
            $("#userinput").get(0).userInfo = userInfo;
        } else {
            $(".mainuserdetails div div,.mainuserdetails div span").fadeOut("slow");
        }
    }
}

function searchUsers(request, response) {
    var apiAddress = findApiAddress(selectsite.val());


    if (apiAddress) {
        $.ajax({
            url: "http://" + apiAddress + "/" + apiVersion + "/users/?filter=" + request.term + "&pagesize=7&page=1&key=" + apiKey,
            dataType: "jsonp",
            jsonp: "jsonp",
            success: function (data, status, xhr) {
                var suggestions = JSLINQ(data.users).Select(function (item) { return item.display_name; }).items;
                window.userData = JSLINQ(data.users).Select(function (item) {
                    return apiUserToUserInfo(item);
                }).items;
                response(suggestions);
            }
        });
    } else {
        response([]);
    }
}

function apiUserToUserInfo(item) {
    return {
        Name: item.display_name,
        GravatarUrl: "http://www.gravatar.com/avatar/" + item.email_hash + "?d=identicon&r=PG",
        GoldBadges: item.badge_counts.gold,
        SilverBadges: item.badge_counts.silver,
        BronzeBadges: item.badge_counts.bronze,
        Rep: item.reputation,
        UserId: item.user_id,
        RepString: formatRep(item.reputation)
    }
}

function getUserInfo(user) {
    var info;
    if (window.userData) {
        $.each(window.userData, function (index, item) {
            if (item.Name == user) {
                info = item;
                return false;
            }
        });
    }
    return info;
}
var selectsite;
var selectUser;

function findSiteInfo(site, member) {
    var site = site.toLowerCase();
    var image;
    $.each(window.siteData, function (index, item) {
        if (item.Site == site)
            image = item[member];
    });
    return image;
}

function findImage(site) {
    return findSiteInfo(site, "SiteImage");
}

function findApiAddress(site) {
    return findSiteInfo(site, "ApiAddress");
}

function onsite(event, b) {
    var val = b ? b.item.value : selectsite.val();
    onuserinput(val, getUserInfo(selectUser.val()));
}
function onuser(event, b) {
    var val = b ? b.item.value : selectUser.val();
    onuserinput(selectsite.val(), getUserInfo(val));
}

function onuserinput(site, userInfo) {
    var hasSite = site != "";
    var hasData = userInfo && hasSite;
    var siteimg = $("#userinput .siteimg");
    if (hasSite) {
        var image = findImage(selectsite.val());
        siteimg.attr("src", image);
        siteimg.load(function () { siteimg.fadeIn("slow"); });
    }
    if (!hasSite && siteimg.css("display") != "none") {
        siteimg.fadeOut("slow");
    }
    updateUserDisplay(userInfo);

    $("#getgraph").button("option", "disabled", !hasData);
}

function createBadge(count, name) {
    if (count) {
        $("<span class='badge' title='" + count + " " + name + " badges'><span class='bullet " + name + "'>&#9679;</span>" + count + "</span>")
                .appendTo("#userinput .userinfo")
    }
}

function initSelect() {
    $("#getgraph").button({ disabled: true });
    selectsite = $("#selectsite").blur(onsite).autocomplete({ select: onsite, source: window.siteSource });
    selectUser = $("#selectuser").blur(onuser).autocomplete({ select: onuser, source: searchUsers });
}


function slugify(str) {
    var stripped = stripVowelAccent(str);
    return stripped.replace(/[^A-Za-z0-9 ]/g, "").replace(/[ ]+/g, "-").toLowerCase().substr(0,81);
}

/* (C)Stephen Chalmers
* Strips grave, acute & circumflex accents from vowels
* Adjusted by Igor Zevaka to strip more accented characters
* http://bytes.com/topic/javascript/answers/145532-replace-french-characters-form-input
*/

function stripVowelAccent(str) {
    var s = str;

    var rExps = [/[\xC0-\xC4]/g, /[\xE0-\xE5]/g,
/[\xC8-\xCB]/g, /[\xE8-\xEB]/g,
/[\xCC-\xCF]/g, /[\xEC-\xEF]/g,
/[\xD2-\xD6]/g, /[\xF2-\xF6]/g,
/[\xD9-\xDC]/g, /[\xF9-\xFC]/g];

    var repChar = ['A', 'a', 'E', 'e', 'I', 'i', 'O', 'o', 'U', 'u'];

    for (var i = 0; i < rExps.length; i++)
        s = s.replace(rExps[i], repChar[i]);

    return s;
}
$.address.externalChange(function (event) {
    if (event.pathNames.length < 2)
        return;

    var site = event.pathNames[0];
    var user = event.pathNames[1];

    selectsite.val(site);
    onsite();
    updateUserFromId(site, user);
    
});
$.address.change(function (event) {
    if (event.pathNames.length < 2)
        return;

    var site = event.pathNames[0];
    var user = event.pathNames[1];
    var addr = formatDataUrl(site, user);

    //Remove the tag stack
    $("#tagstack .animated").css("display", "none");
    $("#tagstackinner div").empty();

    //Clear out previous graph
    //Block graph
    $("#graph").empty().block({ css: {
        'font-family': 'Helvetica,Arial,sans-serif',
        border: 'none',
        padding: '15px',
        backgroundColor: '#000',
        opacity: .5,
        color: '#fff'
    },
        message: "Graph Loading, this may take a while, especially if you are Jon Skeet"
    });

    //Get timeseries from the server


    $.ajax({
        url: addr,
        dataType: "json",
        success: function (data, status, xhr) {

            $.each(data.Labels, function (index, value) { value.showMarker = false; });
            var data = data; //redifine in this scope
            var plot = $.jqplot('graph', data.Timeseries, {
                title: 'Tag Activity',
                legend: { show: false },
                axes: { xaxis: { renderer: $.jqplot.DateAxisRenderer }, yaxis: { autoscale: true} },
                series: data.Labels,
                highlighter: { callback: function (plot, series, neighbour) {
                    var info = data.TimeseriesInfo[series.index][neighbour.pointIndex];
                    return $.jqplot.sprintf(
                                "<strong>%s</strong><br />Week beginning %s<br /><strong>Total: %d</strong><br />Questions: %d<br />Answers: %d<br />Comments: %d<br />",
                                series.label, (new Date(neighbour.data[0])).toDateString(), info.Count, info.QuestionCount, info.AnswerCount, info.CommentCount);
                }
                }
            });

            createTags(plot.legend._series, data.Labels);

            $("#graph").unblock();
        },
        error: function (xhr, textStatus, error) {
            delete $.blockUI.defaults.growlCSS['-moz-border-radius'];
            delete $.blockUI.defaults.growlCSS['-webkit-border-radius'];

            var errorString = $.parseJSON(xhr.responseText).message;
            $.growlUI('Error retreiving data', errorString);
            $("#graph").unblock();
        }
    });

});

$(document).ready(function () {
    initSelect();
    $("#getgraph").click(function () {

        var apiAddress = selectsite.val();
        var userinfo = getUserInfo(selectUser.val());
        console.log("getgraph, userInfo:");
        console.log("userinfo");
        if (!userinfo)
            return;

        $.address.path(apiAddress + "/" + userinfo.UserId + "/" + slugify(userinfo.Name));
    });
});
