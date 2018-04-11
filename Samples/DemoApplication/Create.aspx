<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Create.aspx.cs" EnableEventValidation="true" Inherits="DemoApplication.Create" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Create <%=String.IsNullOrEmpty(Request.QueryString["entityType"]) ? "" : Request.QueryString["entityType"].ToUpper()%></title>
    <link href="Content/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="Content/demo.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <p>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
                    
                    
        &nbsp;</p>
    <div class="container-fluid">
    <asp:Panel runat="server" ID="panelWithEntity">
        <div class="row">
            <div class="col-xs-6 col-xs-offset-3">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <div class="panel-title">
                            Create new <%=Request.QueryString["entityType"].ToUpper() %></div>
                    </div>
                </div>
                <div class="panel-body">
                    <asp:Panel ID="PlaceHolder1" runat="server">
                    </asp:Panel>
                </div>
            </div>
        </div>
                <div class="row">
            <div class="col-xs-6 col-xs-offset-3">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <div class="panel-title">
                            All <%=Request.QueryString["entityType"].ToUpper() %></div>
                    </div>
                </div>
                <div class="panel-body">
                    <asp:GridView runat="server" ID="gridList" 
            CssClass="table table-hover" DataKeyNames="GUID" 
            onselectedindexchanged="gridList_SelectedIndexChanged">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="Button1" runat="server" CommandName="Select" Text="Edit" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
        </asp:GridView>
                </div>
            </div>
        </div>
        </asp:Panel> 
        <asp:Panel runat="server" ID="panelWithoutEntity" Visible="false">
            <div class="alert alert-info" runat="server" id="divErrMsg">
                Provide Url with <b>'entityType'</b>.<br />
                for e.g. ?entityType=student
                
            </div>
        </asp:Panel>
    </div>
    </div>
    </form>
</body>
</html>
