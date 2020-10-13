using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IVPD.Helpers
{
    public class APIResponse
    {
        public bool success { get; set; }
        public object data { get; set; }
        public string message { get; set; }
        public APIResponse() { }

        public APIResponse(bool s, object d, string m)
        {
            success = s;
            data = d;
            message = Translate.TranslateText(m);
        }

    }

    public class Pagination
    {
        public long Limit { get; set; }
        public long CurrentPage { get; set; }
        public long Total { get; set; }
    }

    public class APIListResponse
    {
        public bool success { get; set; }
        public object data { get; set; }
        public Pagination pagination { get; set; }
        public string message { get; set; }
        public APIListResponse(bool s, object d, Pagination p, string m)
        {
            success = s;
            data = d;
            pagination = p;
            message = Translate.TranslateText(m);
        }
    }

    public class FilterClass
    {
        public Dictionary<string, object> Filters { get; set; }
        public string SortBy { get; set; }
        public bool? IsSortTypeDESC { get; set; }
        public bool? IsPagination { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }

    public class FilterClassEntity
    {
        public string modulename { get; set; }
        public Dictionary<string, string> Filters { get; set; }
        public string SortBy { get; set; }
        public bool? IsSortTypeDESC { get; set; }
        public bool? IsPagination { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }

    public static class Translate
    {
        public static string TranslateText(string input)
        {
            try
            {
                Dictionary<string, string> languageConversion = new Dictionary<string, string>();
                languageConversion.Add("Estatuto Created Successfully", "Estatuto criado com sucesso");
                languageConversion.Add("Not able to create estatuto", "Não é possível criar estatuto");
                languageConversion.Add("Estatuto delete successfully!", "Excluir estatuto com sucesso!");
                languageConversion.Add("Estatuto List Returned!", "Lista de estatutos retornada!");
                languageConversion.Add("Permission is denied.Please contact Administrator!", "A permissão é negada.Entre em contato com o administrador!");

                languageConversion.Add("Countries Listed", "Países listados");
                languageConversion.Add("Districts Listed", "Distritos listados");
                languageConversion.Add("Parish Listed", "Paróquia Listada");
                languageConversion.Add("Parcel delete successfully!", "Parcela delete com sucesso");
                languageConversion.Add("Group List Returned!", "Lista de grupos retornada");
                languageConversion.Add("Group Deleted", "Grupo excluído");
                languageConversion.Add("assigned user to a group", "usuário designado a um grupo");

                languageConversion.Add("Legal Situations  Listed", "Situações legais listadas");
                languageConversion.Add("Plot Situation Listed", "Situação do lote listada");
                languageConversion.Add("ClassPlantation Listed", "ClassPlantation Listado");
                languageConversion.Add("LitigationSituation Listed", "Situação listada");
                languageConversion.Add("ExplorerType Listed", "ExplorerType Listado");
                languageConversion.Add("DouroPort Listed", "DouroPort Listado");
                languageConversion.Add("colors Listed", "cores Listadas");
                languageConversion.Add("synonyms Listed", "Sinônimos listados");
                languageConversion.Add("Audit Log Listed", "Log de auditoria listado");
                languageConversion.Add("Comment inserted successfully!", "Comentário inserido com sucesso!");
                languageConversion.Add("Not insert comment. Please try again!", "Não inserir comentário. Por favor, tente novamente!");

                languageConversion.Add("Please send Audit Log ID!", "Envie o ID do log de auditoria!");
                languageConversion.Add("BusEntConcelho Listed", "BusEntConcelho Listado");
                languageConversion.Add("BusEntDistrito Listed", "BusEntDistrito Listado");
                languageConversion.Add("BusEntFreguesiaData List Returned", "BusEntFreguesiaData List Returned");
                languageConversion.Add("BusEntidadeEstatutoData List Returned", "Entidade Estatuto Lista de dados retornada");
                languageConversion.Add("DesignationData List Returned", "DesignaçãoLista de dados retornada");
                languageConversion.Add("Entity List Returned", "Lista de entidades retornada");
                languageConversion.Add("Permission is allowed!", "Permissão é permitida!");
                languageConversion.Add("Permission is not allowed!", "A Entidade não tem estatuto para esta operação");
                languageConversion.Add("Group Created Successfully", "Grupo criado com sucesso");
                languageConversion.Add("Group Updated Successfully", "Grupo atualizado com sucesso");
                languageConversion.Add("Users Listed By GroupID", "Usuários listados por GroupID");
                languageConversion.Add("User assigned to group", "Usuário atribuído ao grupo");
                languageConversion.Add("Groups listed ", "Grupos listados");
                languageConversion.Add("No Data in Group Table", "Sem dados na tabela de grupo");
                languageConversion.Add("GroupPermission Created Successfully", "GroupPermission Criada com Sucesso");
                languageConversion.Add("GroupPermission Deleted Successfully", "GroupPermission excluído com sucesso");
                languageConversion.Add("GroupPermission Listed", "GroupPermission Listado");
                languageConversion.Add("Languages returned Successfully", "Idiomas retornados com sucesso");
                languageConversion.Add("No UserName Exist", "Não existe nome de usuário");
                languageConversion.Add("User Information with details.", "Informações do usuário com detalhes.");
                languageConversion.Add("User Information not found.", "Informações do usuário não encontradas.");

                languageConversion.Add("Username is incorrect", "Nome de usuário incorreto");
                languageConversion.Add("Password Updated Successfully", "Senha atualizada com sucesso");
                languageConversion.Add("Username is not authenticated", "Nome de usuário não autenticado");
                languageConversion.Add("Email Null", "Nulo por email");
                languageConversion.Add("Parcel Created Successfully", "Pacote criado com sucesso");
                languageConversion.Add("Not able to create parcel", "Não é possível criar parcela");
                languageConversion.Add("Data Updated Succesfully", "Dados atualizados com sucesso");
                languageConversion.Add("User Created Successfully", "Usuário criado com sucesso");
                languageConversion.Add("User Updated Successfully", "Usuário atualizado com sucesso");
                languageConversion.Add("User Type Listed", "Tipo de usuário listado");

                languageConversion.Add("User Listed", "Usuário listado");
                languageConversion.Add("Parcel List Returned", "Lista de encomendas devolvida");
                languageConversion.Add("User Listed By Id", "Usuário listado por ID");
                languageConversion.Add("user Deleted", "usuário excluído");


                languageConversion.Add("Permissions return by Group Successfully ", "Permissões retornadas pelo grupo com êxito");
                languageConversion.Add("Permission List Returned", "Lista de permissões retornada");

                languageConversion.Add("List Returned", "Lista retornada");
                languageConversion.Add("No Permissions associated with the group id", "Nenhuma permissão associada ao ID do grupo");

                languageConversion.Add("Login SSO URL successfully generated!", "URL de logon SSO gerado com sucesso!");
                languageConversion.Add("Something went wrong!", "Algo deu errado!");
                languageConversion.Add("Token generated successfully!", "Token gerado com sucesso!");
                languageConversion.Add("Token not generated!", "Token não gerado!");

                languageConversion.Add("Password is required", "Senha requerida");
                languageConversion.Add("FullName is required", "Nome completo é obrigatório");
                languageConversion.Add("Email is required", "E-mail é obrigatório");
                languageConversion.Add("Display Name is required", "O nome de exibição é obrigatório");
                languageConversion.Add("Contact Number is required", "O número de contato é obrigatório");
                languageConversion.Add("Date of Birth is required", "Data de nascimento é obrigatória");
                languageConversion.Add("Notes is required", "É necessário fazer anotações");
                languageConversion.Add("Email is not in correct format", "O email não está no formato correto");
                languageConversion.Add("Designationid is required", "O ID da designação é obrigatório");
                languageConversion.Add("password", "senha");
                languageConversion.Add("Value cannot be empty or whitespace only string.", "O valor não pode estar vazio ou apenas com espaço em branco.");
                languageConversion.Add("Invalid length of password hash (64 bytes expected).", "Comprimento inválido do hash da senha (64 bytes esperados).");
                languageConversion.Add("Invalid length of password salt (128 bytes expected).", "Comprimento inválido da senha salt (128 bytes esperados).");
                languageConversion.Add("Id is Required", "Id é obrigatório");
                languageConversion.Add("Schedule Cancelled Successfully and Mail Sent Successfully!", "Programação cancelada com sucesso e email enviado com sucesso!");
                languageConversion.Add("Schedule Cancelled Successfully and Mail Not Sent !", "Programação cancelada com sucesso e email não enviado!");
                languageConversion.Add("Schedule Cancelled Successfully!", "Programação cancelada com sucesso!");
                languageConversion.Add("no Schedule Exist with the given id", "Não existe programação com o ID fornecido");
                languageConversion.Add("Schedule Resend Successfully and Mail Sent Successfully!", "Agende Reenviar com Sucesso e Correio Enviado com Sucesso!");
                languageConversion.Add("Schedule Resend Successfully and Mail Not Sent !", "Agende o reenvio com sucesso e o email não foi enviado!");
                languageConversion.Add("Schedule Resend Successfully!", "Agendar Reenviar com Sucesso!");
                languageConversion.Add("Schedule Approved Successfully and Mail Sent Successfully!", "Programação aprovada com sucesso e email enviado com sucesso!");
                languageConversion.Add("Schedule Approved Successfully and Mail Not Sent !", "Programação aprovada com sucesso e email não enviado!");
                languageConversion.Add("Schedule Approved Successfully!", "Programação aprovada com sucesso!");
                languageConversion.Add("No Schedule Found !", "Nenhuma programação encontrada!");
                languageConversion.Add("Schedule Created Successfully and Mail Sent Successfully!", "Agenda criada com sucesso e email enviado com sucesso!");
                languageConversion.Add("Schedule Created Successfully and Mail Not Sent !", "Agenda criada com sucesso e email não enviado!");
                languageConversion.Add("Schedule Created Successfully!", "Programação criada com sucesso!");
                languageConversion.Add("Schedule Title is required!", "O título da programação é obrigatório!");
                languageConversion.Add("Schedule Entity is required!", "Entidade de programação é necessária!");
                languageConversion.Add("Schedule Observation is required!", "Agendar observação é necessária!");
                languageConversion.Add("Schedule Updated Successfully and Mail Sent Successfully!", "Agenda atualizada com sucesso e email enviado com sucesso!");
                languageConversion.Add("Schedule Updated Successfully and Mail Not Sent !", "Agenda atualizada com sucesso e email não enviado!");
                languageConversion.Add("Schedule Updated Successfully!", "Programação atualizada com sucesso!");
                languageConversion.Add("Success", "Sucesso");
                languageConversion.Add("Fail", "Falhou");
                languageConversion.Add("Link sent on registered email.Please login and verify", "Link enviado por e-mail registrado. Faça o login e verifique");
                languageConversion.Add("User not found. Please provide a valid user name.", "Utilizador não encontrado. Indique um nome de utilizador válido.");
                languageConversion.Add("User Name can not be empty!Please provide the user name.", "O nome do usuário não pode estar vazio! Forneça o nome do usuário.");
                languageConversion.Add("User not found", "Usuário não encontrado");
                languageConversion.Add("Name is required", "Nome é obrigatório");
                languageConversion.Add("Description is required", "A descrição é obrigatória");
                languageConversion.Add("Status is required", "O status é obrigatório");
                languageConversion.Add("No group Exist with the given id", "Nenhum grupo existe com o ID fornecido");
                languageConversion.Add("No group Exist with the given id to remove", "Não existe nenhum grupo com o ID fornecido para remover");
                languageConversion.Add("GroupId is Required", "O ID do grupo é obrigatório");
                languageConversion.Add("No User exist attached with user id", "Nenhum usuário existe anexado com o ID do usuário");
                languageConversion.Add("No User Exist with the given id", "Nenhum usuário existe com o ID fornecido");
                languageConversion.Add("No user Group exist with given group id and user id", "Nenhum grupo de usuários existe com o ID do grupo e o ID do usuário");
                languageConversion.Add("Group id Should be other that zero", "A identificação do grupo deve ser diferente de zero");
                languageConversion.Add("Username or password is incorrect", "Nome de utilizador ou palavra-passe incorreta");
                languageConversion.Add("Login Success", "Sucesso no login");


                languageConversion.Add("Entity already exist!", "A entidade já existe!");
                languageConversion.Add("Data saved successfully!", "Dados salvos com sucesso!");
                languageConversion.Add("Data updated successfully!", "Dados atualizados com sucesso!");
                languageConversion.Add("Data deleted successfully!", "Dados excluídos com sucesso!");
                languageConversion.Add("Something went wrong. Please try again later!", "Algo deu errado. Por favor, tente novamente mais tarde!");

                languageConversion.Add("No Schedule in Database with this ID", "Nenhuma programação no banco de dados com esse ID!");
                languageConversion.Add("Schedule returned Successfully", "Agenda retornada com sucesso!");

                languageConversion.Add("No Parcel Exist with the given id!", "Não existe nenhum parcela com o ID fornecido!");
                languageConversion.Add("Parcel Data returned!", "Dados do parcela retornados!");

                languageConversion.Add("Reconcile Producer Data List Returned", "Reconciliar lista de dados do produtor retornada!");
                languageConversion.Add("Transfer Producer Data List Returned", "Lista de dados do produtor transferido retornada!");
                languageConversion.Add("Log Files Data List Returned", "Lista de dados dos arquivos de log retornada!");
                languageConversion.Add("Payment Made Data List Returned", "Lista de dados do pagamento efetuado retornada!");
                languageConversion.Add("Entities Difference Data List Returned", "Lista de dados de diferença de entidades retornada!");
                languageConversion.Add("Payment Not Sent to DGT Data List Returned", "Pagamento não enviado à lista de dados DGT retornada");
                languageConversion.Add("Reconcile Carried Data List Returned", "Reconciliar lista de dados transportados retornada");
                languageConversion.Add("Outstanding Payment Data List Returned", "Lista de dados de pagamento pendentes retornada");
                languageConversion.Add("Payment Held List Returned", "Lista de pagamentos retidos retornada");
                languageConversion.Add("Payment Returned List Returned", "Lista de pagamentos devolvidos devolvidos!");
                languageConversion.Add("Pending Files Data List Returned", "Lista de dados de arquivos pendentes retornada!");
                languageConversion.Add("Consultation Active Pledges List Returned", "Lista de promessas ativas da consulta retornada");
                languageConversion.Add("Payment Entities Data List Returned", "Lista de dados das entidades de pagamento retornada!");
                languageConversion.Add("Payment Dico Fre Data List Returned", "Lista de dados de pagamento Dico Fre retornada!");
                languageConversion.Add("Receipts Data List Returned", "Lista de dados de recebimentos retornada!");
                languageConversion.Add("Pending Payment List Returned", "Lista de pagamentos pendentes devolvida!");
                languageConversion.Add("Income Statements Data List Returned", "Lista de dados da demonstração de resultados retornada!");
                languageConversion.Add("File Detail Data List Returned", "Lista de dados detalhados do arquivo retornada");
                languageConversion.Add("Payment File Detail Data List Returned", "Lista de dados detalhados do arquivo de pagamento retornada");
                languageConversion.Add("Register Movements Data List Returned", "Registre a lista de dados de movimentos retornada");
                languageConversion.Add("Registration Impression Data List Returned", "Lista de dados de impressão de registro retornada");
                languageConversion.Add("Payment Confirmation Data List Returned", "Lista de dados de confirmação de pagamento retornada");
                languageConversion.Add("All LQBase Data List Returned", "Toda a lista de dados da base LQ retornada");
                languageConversion.Add("MGBalance List Returned", "Lista MGBalance retornada");
                languageConversion.Add("Situcao Da Parcela Listed", "Situcao Da Parcela Listado");
                languageConversion.Add("Sinonimo Data List Returned", "Lista de dados Sinonimo retornada!");
                languageConversion.Add("Casta Listed", "Casta Listado");
                languageConversion.Add("ImportedPaymentFiles Data List Returned", "Lista de dados de arquivos de pagamento importados retornada!");
                languageConversion.Add("FilesCreateBank Data List Returned", "Arquivos Criar lista de dados bancários retornada!");
                languageConversion.Add("NUMPARC is already exist!", "NUMPARC já existe!");
                languageConversion.Add("InformationYear Data List Returned", "Lista de dados do ano de informações retornada!");
                languageConversion.Add("ProducerAccount Data List Returned", "Lista de dados da conta do produtor retornada!");
                languageConversion.Add("Register Printing Created Successfully.", "Registre a impressão criada com sucesso");
                languageConversion.Add("UnfoldTransfer inserted successfully", "Desdobrar Transferência inserida com sucesso!");
                languageConversion.Add("SidePanel inserted successfully", "Painel lateral inserido com sucesso!");
                languageConversion.Add("Not able to create Register Printing", "Não é possível criar a impressão do registro");
                languageConversion.Add("Tipo Data List Returned", "Tipo Lista de dados retornada!");
                languageConversion.Add("An error occurred while updating the entries. See the inner exception for details.", "Você inseriu os dados incorretos. Digite os dados corretos!");

                languageConversion.Add("Date is not a current date", "A data não é uma data atual");
                languageConversion.Add("Created Successfully", "Criado com sucesso");
                languageConversion.Add("Updated Successfully", "Atualizado com sucesso");
                languageConversion.Add("You should open cashier before proceeding this request", "Você deve abrir o caixa antes de prosseguir com esta solicitação");
                languageConversion.Add("Alotted Service Created Successfully", "Serviço Alotted Criado com Sucesso");
                languageConversion.Add("Not able to create Alotted Service", "Não é possível criar serviço Alotted");
                languageConversion.Add("You should close cashier before proceeding this request", "Você deve fechar o caixa antes de prosseguir com esta solicitação");

                languageConversion.Add("User Status is Inactive", "O status do usuário está inativo");
                languageConversion.Add("Levantamento Data List Returned", "Lista de dados retornada!");
                languageConversion.Add("Production Authorization Listed", "Lista de autorização de produção");
                languageConversion.Add("Collection Revenue Listed", "Receita de coleção listada");
                languageConversion.Add("No List", "Sem lista");
                languageConversion.Add("No Address Exist with the given Entity id!", "Nenhum endereço existe com o ID de entidade fornecido!");
                languageConversion.Add("Billing Address returned!", "Endereço de cobrança devolvido!");
                languageConversion.Add("No Previous Balance  with the given date exist!", "Não existe saldo anterior com a data indicada!");
                languageConversion.Add("Previous Balance Returned!", "Saldo anterior devolvido!");
                languageConversion.Add("No Balance in cash  with the given date and Entity exist!", "Não existe saldo em dinheiro com a data e a entidade fornecidas!");
                languageConversion.Add("Balance in Cash Returned!", "Endereço de cobrança devolvido!");
                languageConversion.Add("No Balance  with the given date and Entity exist!", "Não existe saldo com a data e a entidade fornecidas!");
                languageConversion.Add("Balance in Check Returned!", "Saldo em cheque devolvido!");
                languageConversion.Add("Data Inserted Succesfully", "Dados inseridos com sucesso");
                languageConversion.Add("All Transaction List Returned", "Todas as listas de transações devolvidas");
                languageConversion.Add("No Data Return", "Sem retorno de dados");
                languageConversion.Add("Data Not Updated", "Dados não atualizados");
                languageConversion.Add("Details Returned!", "Detalhes devolvidos!");
                languageConversion.Add("No Data Exists!", "Não existem dados!");
                languageConversion.Add("Details Not Returned!", "Detalhes não devolvidos!");
                languageConversion.Add("Data returned!", "Dados retornados!");
                languageConversion.Add("Transaction Method Listed", "Método de transação listado");
                languageConversion.Add("Transaction Deleted", "Transação excluída");
                languageConversion.Add("Entity Account Details Listed", "Detalhes da conta da entidade listados");
                languageConversion.Add("Currency Listed", "Moeda listada");

                
                string value = languageConversion.Where(w => w.Key == input).Select(s => s.Value).FirstOrDefault();
                if (value == null)
                {
                    value = input;
                    if (input.Contains("A connection attempt failed because the connected party did not properly respond after a period of time"))
                    {
                        value = "Ocorreu um erro com o seu pedido. Por favor, contate o administrador ou tente mais tarde";
                    }
                    if (input.Contains("GroupPermission not found with id"))
                    {
                        value = value.Replace("GroupPermission not found with id", "Permissão de grupo não encontrada com o ID");
                    }
                    else if (input.Contains("Permission not found with id"))
                    {
                        value = value.Replace("Permission not found with id", "Permissão não encontrada com o ID");
                    }
                    else if (input.Contains("group not found with id"))
                    {
                        value = value.Replace("group not found with id", "grupo não encontrado com o ID");
                    }
                    else if (input.Contains("Group Name"))
                    {
                        value = value.Replace("Group Name", "Nome do grupo");
                        value = value.Replace("is already taken", "já está tomado");
                    }
                    else if (input.Contains("Email"))
                    {
                        value = value.Replace("Email", "O email");
                        value = value.Replace("is already taken", "já está tomado");
                    }
                    else if (input.Contains("Username"))
                    {
                        value = value.Replace("Username", "Nome do usuário");
                        value = value.Replace("is already taken", "já está tomado");
                    }

                    else if (input.Contains("field is required"))
                    {
                        value = value.Replace("field is required", "campo é obrigatório");
                    }

                    else if (input.Contains("The JSON value could not be converted"))
                    {
                        value = "Digite o valor correto!";
                    }

                    else if (input.Contains("must be a string or array type with a maximum length of"))
                    {
                        string[] arr = input.Split('\'');
                        int length = 0;
                        if (arr.Length > 0)
                        {
                            string io = arr[1].Trim();
                            io = io.Replace("'", "");
                            int.TryParse(io, out length);
                        }

                        value = "Este campo não pode exceder " + length + " comprimentos!";
                    }
                }
                return value;
            }
            catch (Exception ex)
            {
                return input;
            }

        }

    }


}
