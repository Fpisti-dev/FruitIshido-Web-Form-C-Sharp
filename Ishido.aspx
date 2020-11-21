<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Ishido.aspx.cs" Inherits="Ishido" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Fruit Ishido</title>
    <meta name="viewport" content="width=device-width,initial-scale=1"/> 

    <script src="/Scripts/js/jquery-1.12.4.js"></script>
    <script src="/Scripts/js/jquery-ui.min.js" type = "text/javascript"></script>

    <link href="/Scripts/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Scripts/css/style.css" rel="stylesheet" />
    
    <script src="/Scripts/js/bootstrap.min.js"></script>     

</head>
<body>
    <form id="form1" runat="server">

        <div class="container">

            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>  

            <nav class="navbar navbar-expand-lg navbar-light bg-light">
                
                <a class="navbar-brand" href="#">Fruit Ishido</a>
                
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
  
                <div class="collapse navbar-collapse" id="navbarNav">
    
                    <ul class="navbar-nav">
      
                        <li class="nav-item">
                            <asp:Button class="btn" style="margin-left: 2em" ID="btnTop10" runat="server" Text="TOP 10" OnClick="btnTop10_Click" />
                        </li>
      
                        <li class="nav-item">
                            <asp:Button class="btn" style="margin-left: 2em" ID="btnManual" runat="server" Text="Manual" OnClick="btnManual_Click" />
                        </li>

                        <li class="nav-item">
                            <asp:Button class="btn" style="margin-left: 2em" ID="btnAbout" runat="server" Text="About" OnClick="btnAbout_Click" />
                        </li>

                    </ul>
                </div>
            </nav>

            <div class="row" style="margin-top: 2em">
    
                <div class="col-sm-5">

                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblMessageDisplay" runat="server" Text="Messages:"></asp:Label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtMessageDisplay" class="form-control" runat="server" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-4 col-form-label" ID="lblNextStone" runat="server" Text="Next Stone"></asp:Label>
                        <div class="col-sm-8">
                            <asp:Button ID="btnNextStone" runat="server" Text="" Height="50" Width="50" Enabled="False" />
                        </div>
                    </div>

                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-4 col-form-label" ID="lblUsedStoneDisplay" runat="server" Text="Used Stones:"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtUsedStoneDisplay" class="form-control" runat="server" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-4 col-form-label" ID="lblLeftStoneDisplay" runat="server" Text="Left Stones:"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtLeftStoneDisplay" class="form-control" runat="server" ReadOnly="True"></asp:TextBox>
                        </div>
                        
                    </div>

                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-4 col-form-label" ID="lblScoreDisplay" runat="server" Text="Score:"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtScoreDisplay" class="form-control" runat="server" ReadOnly="True"></asp:TextBox>
                        </div>
                        
                    </div>

                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-4 col-form-label" ID="lblFourWayDisplay" runat="server" Text="Four Ways:"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtFourWayDisplay" class="form-control" runat="server" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group row">
                        
                        <div class="col-sm-3">
                            <asp:Button ID="btnNewGame" runat="server" Text="New Game" OnClick="btnNewGame_Click" OnClientClick="return confirm('Are you sure?')" />
                        </div>                      

                    </div>

                    <div class="form-group row">

                        <div class="col-sm-4">
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return confirm('Are you sure?')"/>
                        </div>

                        <div class="col-sm-8">
                            <asp:FileUpload id="FileUploadControl" runat="server" />
                            <asp:Button ID="btnLoad" style="margin-top: 1em" runat="server" Text="Load" OnClick="btnLoad_Click" OnClientClick="return confirm('Are you sure?')"/>
                        </div>

                    </div>

                </div>

                <div class="col-sm-7">
                    <asp:Panel ID="panGame" runat="server" Height="400px" Width="600px"></asp:Panel>
                </div>
        
    
                </div>
        
            </div>

        <!-- Bootstrap Modal Dialog -->
        <div class="modal fade" id="topModal" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <asp:UpdatePanel ID="upModal" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4 class="modal-title"><asp:Label ID="lblModalTitle" runat="server" Text="TOP 10"></asp:Label></h4>
                                <div class="form-group row">
                                        <asp:Label ID="lblYourScore" class="col-sm-3" runat="server" Text="Your score:"></asp:Label>
                                        <asp:TextBox ID="txtYourScore" class="col-sm-2 form-control" runat="server" ReadOnly="True"></asp:TextBox>
                                    </div>
                            </div>

                            <div class="modal-body">                                
                                <asp:Panel ID="pnlTop10" runat="server"></asp:Panel>

                                <asp:Panel ID="pnlInTop10" runat="server">
                                    <div class="form-group row">
                                        <asp:Label ID="lblNickName" class="col-sm-3" runat="server" Text="Nick name:"></asp:Label>
                                        <asp:TextBox ID="txtNickName" class="col-sm-4 form-control" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnSaveTop10" style="margin-left: 1em;" class="col-sm-2 btn btn-sm btn-success" runat="server" Text="Save" OnClick="btnSaveTop10_Click" />
                                    </div>

                                    <div class="form-group row">
                                        <div class="col-sm-3"></div>
                                        <asp:Label ID="lblNickNameRequired" class="col-sm-6" runat="server" Text="Nick name required !" ForeColor="Red" Font-Size="Smaller"></asp:Label>
                                    </div>
                                    
                                </asp:Panel>
                            </div>
                    
                            <div class="modal-footer">
                                <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <!-- Bootstrap Modal Dialog -->
        <div class="modal fade" id="manModal" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                        <div class="modal-content" style="width: 150%;">
                            <div class="modal-header">
                                <h4 class="modal-title"><asp:Label ID="lblUserManual" runat="server" Text="User Manual"></asp:Label></h4>
                            </div>

                            <div class="modal-body">
                                <asp:Panel ID="pnlManual" runat="server"></asp:Panel>
                                <div class="form-group row">
                                    <div class="col-sm-12">
                                        <div class="pull-left">
                                    <asp:Button ID="btnPrevPage" class="col-sm-2 btn btn-sm btn-success float-left" runat="server" Text="<< Prev" OnClick="btnPrevPage_Click" />
                                        </div>
                                        <div class="pull-right">
                                            <asp:Button ID="btnNextPage" class="col-sm-2 btn btn-sm btn-success float-right" runat="server" Text="Next >>" OnClick="btnNextPage_Click" />
                                        </div>
                                    </div>                                       
                                </div>
                            </div>
                    
                            <div class="modal-footer">
                                <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                            </div>
                        </div>
            </div>
        </div>

        <!-- Bootstrap Modal Dialog -->
        <div class="modal fade" id="aboModal" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4 class="modal-title"><asp:Label ID="Label3" runat="server" Text="About"></asp:Label></h4>
                            </div>

                            <div class="modal-body">
                                <asp:Label ID="lblAbout1" runat="server" Text="FruitIshdo"></asp:Label>
                                <br />
                                <asp:Label ID="lblAbout2" runat="server" Text="Version 1.0.0.0"></asp:Label>
                                <br />
                                <asp:Label ID="lblAbout3" runat="server" Text="Copyright &copy; 2019"></asp:Label>
                                <br />
                                <asp:Label ID="lblAbout4" runat="server" Text="The best logic game"></asp:Label>
                                <br />
                            </div>
                    
                            <div class="modal-footer">
                                <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    
    </form>
</body>
</html>
