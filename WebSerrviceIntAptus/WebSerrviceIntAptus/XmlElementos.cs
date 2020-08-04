using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSerrviceIntAptus
{
    public class XmlElementos
    {


        // OBSERVAÇÃO: o código gerado pode exigir pelo menos .NET Framework 4.5 ou .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class Relatorios
        {

            private RelatoriosRelatorioDespesa[] relatorioDespesaField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("RelatorioDespesa")]
            public RelatoriosRelatorioDespesa[] RelatorioDespesa
            {
                get
                {
                    return this.relatorioDespesaField;
                }
                set
                {
                    this.relatorioDespesaField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class RelatoriosRelatorioDespesa
        {

            private byte codigoField;

            private uint dataCriacaoField;

            private uint dataEnvioField;

            private uint dataPrevisaoPagamentoField;

            private bool dataPrevisaoPagamentoFieldSpecified;

            private uint dataUltimaAprovacaoField;

            private byte ultimoAprovadorCodigoField;

            private string ultimoAprovadorDescricaoField;

            private string statusCodigoField;

            private string statusDescricaoField;

            private byte solicitanteIdField;

            private object solicitanteCodigoField;

            private string usuarioCodigoField;

            private string solicitanteNomeField;

            private byte empresaCodigoField;

            private byte empresaCodigoERPField;

            private object moedaCodigoField;

            private object moedaDescricaoField;

            private decimal valorTotalField;

            private decimal valorReembolsoField;

            private decimal valorAdiantamentoField;

            private decimal cotacaoIndiceField;

            private decimal valorTotalIndiceField;

            private decimal valorReembolsoIndiceField;

            private decimal valorAdiantamentoIndiceField;

            private string textoCabecalhoField;

            private byte chaveField;

            private RelatoriosRelatorioDespesaRelatorioDespesaLancamento[] lancamentosField;

            private RelatoriosRelatorioDespesaRelatorioDespesaLancamentoContabil[] lancamentosContabilidadeField;

            private object lancamentosFinanceiroField;

            /// <remarks/>
            public byte Codigo
            {
                get
                {
                    return this.codigoField;
                }
                set
                {
                    this.codigoField = value;
                }
            }

            /// <remarks/>
            public uint DataCriacao
            {
                get
                {
                    return this.dataCriacaoField;
                }
                set
                {
                    this.dataCriacaoField = value;
                }
            }

            /// <remarks/>
            public uint DataEnvio
            {
                get
                {
                    return this.dataEnvioField;
                }
                set
                {
                    this.dataEnvioField = value;
                }
            }

            /// <remarks/>
            public uint DataPrevisaoPagamento
            {
                get
                {
                    return this.dataPrevisaoPagamentoField;
                }
                set
                {
                    this.dataPrevisaoPagamentoField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DataPrevisaoPagamentoSpecified
            {
                get
                {
                    return this.dataPrevisaoPagamentoFieldSpecified;
                }
                set
                {
                    this.dataPrevisaoPagamentoFieldSpecified = value;
                }
            }

            /// <remarks/>
            public uint DataUltimaAprovacao
            {
                get
                {
                    return this.dataUltimaAprovacaoField;
                }
                set
                {
                    this.dataUltimaAprovacaoField = value;
                }
            }

            /// <remarks/>
            public byte UltimoAprovadorCodigo
            {
                get
                {
                    return this.ultimoAprovadorCodigoField;
                }
                set
                {
                    this.ultimoAprovadorCodigoField = value;
                }
            }

            /// <remarks/>
            public string UltimoAprovadorDescricao
            {
                get
                {
                    return this.ultimoAprovadorDescricaoField;
                }
                set
                {
                    this.ultimoAprovadorDescricaoField = value;
                }
            }

            /// <remarks/>
            public string StatusCodigo
            {
                get
                {
                    return this.statusCodigoField;
                }
                set
                {
                    this.statusCodigoField = value;
                }
            }

            /// <remarks/>
            public string StatusDescricao
            {
                get
                {
                    return this.statusDescricaoField;
                }
                set
                {
                    this.statusDescricaoField = value;
                }
            }

            /// <remarks/>
            public byte SolicitanteId
            {
                get
                {
                    return this.solicitanteIdField;
                }
                set
                {
                    this.solicitanteIdField = value;
                }
            }

            /// <remarks/>
            public object SolicitanteCodigo
            {
                get
                {
                    return this.solicitanteCodigoField;
                }
                set
                {
                    this.solicitanteCodigoField = value;
                }
            }

            /// <remarks/>
            public string UsuarioCodigo
            {
                get
                {
                    return this.usuarioCodigoField;
                }
                set
                {
                    this.usuarioCodigoField = value;
                }
            }

            /// <remarks/>
            public string SolicitanteNome
            {
                get
                {
                    return this.solicitanteNomeField;
                }
                set
                {
                    this.solicitanteNomeField = value;
                }
            }

            /// <remarks/>
            public byte EmpresaCodigo
            {
                get
                {
                    return this.empresaCodigoField;
                }
                set
                {
                    this.empresaCodigoField = value;
                }
            }

            /// <remarks/>
            public byte EmpresaCodigoERP
            {
                get
                {
                    return this.empresaCodigoERPField;
                }
                set
                {
                    this.empresaCodigoERPField = value;
                }
            }

            /// <remarks/>
            public object MoedaCodigo
            {
                get
                {
                    return this.moedaCodigoField;
                }
                set
                {
                    this.moedaCodigoField = value;
                }
            }

            /// <remarks/>
            public object MoedaDescricao
            {
                get
                {
                    return this.moedaDescricaoField;
                }
                set
                {
                    this.moedaDescricaoField = value;
                }
            }

            /// <remarks/>
            public decimal ValorTotal
            {
                get
                {
                    return this.valorTotalField;
                }
                set
                {
                    this.valorTotalField = value;
                }
            }

            /// <remarks/>
            public decimal ValorReembolso
            {
                get
                {
                    return this.valorReembolsoField;
                }
                set
                {
                    this.valorReembolsoField = value;
                }
            }

            /// <remarks/>
            public decimal ValorAdiantamento
            {
                get
                {
                    return this.valorAdiantamentoField;
                }
                set
                {
                    this.valorAdiantamentoField = value;
                }
            }

            /// <remarks/>
            public decimal CotacaoIndice
            {
                get
                {
                    return this.cotacaoIndiceField;
                }
                set
                {
                    this.cotacaoIndiceField = value;
                }
            }

            /// <remarks/>
            public decimal ValorTotalIndice
            {
                get
                {
                    return this.valorTotalIndiceField;
                }
                set
                {
                    this.valorTotalIndiceField = value;
                }
            }

            /// <remarks/>
            public decimal ValorReembolsoIndice
            {
                get
                {
                    return this.valorReembolsoIndiceField;
                }
                set
                {
                    this.valorReembolsoIndiceField = value;
                }
            }

            /// <remarks/>
            public decimal ValorAdiantamentoIndice
            {
                get
                {
                    return this.valorAdiantamentoIndiceField;
                }
                set
                {
                    this.valorAdiantamentoIndiceField = value;
                }
            }

            /// <remarks/>
            public string TextoCabecalho
            {
                get
                {
                    return this.textoCabecalhoField;
                }
                set
                {
                    this.textoCabecalhoField = value;
                }
            }

            /// <remarks/>
            public byte Chave
            {
                get
                {
                    return this.chaveField;
                }
                set
                {
                    this.chaveField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("RelatorioDespesaLancamento", IsNullable = false)]
            public RelatoriosRelatorioDespesaRelatorioDespesaLancamento[] Lancamentos
            {
                get
                {
                    return this.lancamentosField;
                }
                set
                {
                    this.lancamentosField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("RelatorioDespesaLancamentoContabil", IsNullable = false)]
            public RelatoriosRelatorioDespesaRelatorioDespesaLancamentoContabil[] LancamentosContabilidade
            {
                get
                {
                    return this.lancamentosContabilidadeField;
                }
                set
                {
                    this.lancamentosContabilidadeField = value;
                }
            }

            /// <remarks/>
            public object LancamentosFinanceiro
            {
                get
                {
                    return this.lancamentosFinanceiroField;
                }
                set
                {
                    this.lancamentosFinanceiroField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class RelatoriosRelatorioDespesaRelatorioDespesaLancamento
        {

            private byte codigoField;

            private object moedaCodigoField;

            private object moedaDescricaoField;

            private decimal cotacaoIndiceField;

            private decimal valorTotalField;

            private decimal valorReembolsoField;

            private decimal valorTotalIndiceField;

            private decimal valorReembolsoIndiceField;

            private string flagDevolucaoField;

            private byte tipoDespesaCodigoField;

            private string tipoDespesaDescricaoField;

            private object projetoCodigoField;

            private object projetoDescricaoField;

            private string comentariosField;

            private object descricaoAutomaticaField;

            private decimal quantidadeKMField;

            private byte quantidadePessoasField;

            private byte quantidadeDiasHospedagemField;

            private object cursoDescricaoField;

            private string textoItemField;

            private uint dataPrevisaoPagamentoField;

            private bool dataPrevisaoPagamentoFieldSpecified;

            private byte lancamentoField;

            private byte chaveField;

            private RelatoriosRelatorioDespesaRelatorioDespesaLancamentoAnexos anexosField;

            /// <remarks/>
            public byte Codigo
            {
                get
                {
                    return this.codigoField;
                }
                set
                {
                    this.codigoField = value;
                }
            }

            /// <remarks/>
            public object MoedaCodigo
            {
                get
                {
                    return this.moedaCodigoField;
                }
                set
                {
                    this.moedaCodigoField = value;
                }
            }

            /// <remarks/>
            public object MoedaDescricao
            {
                get
                {
                    return this.moedaDescricaoField;
                }
                set
                {
                    this.moedaDescricaoField = value;
                }
            }

            /// <remarks/>
            public decimal CotacaoIndice
            {
                get
                {
                    return this.cotacaoIndiceField;
                }
                set
                {
                    this.cotacaoIndiceField = value;
                }
            }

            /// <remarks/>
            public decimal ValorTotal
            {
                get
                {
                    return this.valorTotalField;
                }
                set
                {
                    this.valorTotalField = value;
                }
            }

            /// <remarks/>
            public decimal ValorReembolso
            {
                get
                {
                    return this.valorReembolsoField;
                }
                set
                {
                    this.valorReembolsoField = value;
                }
            }

            /// <remarks/>
            public decimal ValorTotalIndice
            {
                get
                {
                    return this.valorTotalIndiceField;
                }
                set
                {
                    this.valorTotalIndiceField = value;
                }
            }

            /// <remarks/>
            public decimal ValorReembolsoIndice
            {
                get
                {
                    return this.valorReembolsoIndiceField;
                }
                set
                {
                    this.valorReembolsoIndiceField = value;
                }
            }

            /// <remarks/>
            public string FlagDevolucao
            {
                get
                {
                    return this.flagDevolucaoField;
                }
                set
                {
                    this.flagDevolucaoField = value;
                }
            }

            /// <remarks/>
            public byte TipoDespesaCodigo
            {
                get
                {
                    return this.tipoDespesaCodigoField;
                }
                set
                {
                    this.tipoDespesaCodigoField = value;
                }
            }

            /// <remarks/>
            public string TipoDespesaDescricao
            {
                get
                {
                    return this.tipoDespesaDescricaoField;
                }
                set
                {
                    this.tipoDespesaDescricaoField = value;
                }
            }

            /// <remarks/>
            public object ProjetoCodigo
            {
                get
                {
                    return this.projetoCodigoField;
                }
                set
                {
                    this.projetoCodigoField = value;
                }
            }

            /// <remarks/>
            public object ProjetoDescricao
            {
                get
                {
                    return this.projetoDescricaoField;
                }
                set
                {
                    this.projetoDescricaoField = value;
                }
            }

            /// <remarks/>
            public string Comentarios
            {
                get
                {
                    return this.comentariosField;
                }
                set
                {
                    this.comentariosField = value;
                }
            }

            /// <remarks/>
            public object DescricaoAutomatica
            {
                get
                {
                    return this.descricaoAutomaticaField;
                }
                set
                {
                    this.descricaoAutomaticaField = value;
                }
            }

            /// <remarks/>
            public decimal QuantidadeKM
            {
                get
                {
                    return this.quantidadeKMField;
                }
                set
                {
                    this.quantidadeKMField = value;
                }
            }

            /// <remarks/>
            public byte QuantidadePessoas
            {
                get
                {
                    return this.quantidadePessoasField;
                }
                set
                {
                    this.quantidadePessoasField = value;
                }
            }

            /// <remarks/>
            public byte QuantidadeDiasHospedagem
            {
                get
                {
                    return this.quantidadeDiasHospedagemField;
                }
                set
                {
                    this.quantidadeDiasHospedagemField = value;
                }
            }

            /// <remarks/>
            public object CursoDescricao
            {
                get
                {
                    return this.cursoDescricaoField;
                }
                set
                {
                    this.cursoDescricaoField = value;
                }
            }

            /// <remarks/>
            public string TextoItem
            {
                get
                {
                    return this.textoItemField;
                }
                set
                {
                    this.textoItemField = value;
                }
            }

            /// <remarks/>
            public uint DataPrevisaoPagamento
            {
                get
                {
                    return this.dataPrevisaoPagamentoField;
                }
                set
                {
                    this.dataPrevisaoPagamentoField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool DataPrevisaoPagamentoSpecified
            {
                get
                {
                    return this.dataPrevisaoPagamentoFieldSpecified;
                }
                set
                {
                    this.dataPrevisaoPagamentoFieldSpecified = value;
                }
            }

            /// <remarks/>
            public byte Lancamento
            {
                get
                {
                    return this.lancamentoField;
                }
                set
                {
                    this.lancamentoField = value;
                }
            }

            /// <remarks/>
            public byte Chave
            {
                get
                {
                    return this.chaveField;
                }
                set
                {
                    this.chaveField = value;
                }
            }

            /// <remarks/>
            public RelatoriosRelatorioDespesaRelatorioDespesaLancamentoAnexos Anexos
            {
                get
                {
                    return this.anexosField;
                }
                set
                {
                    this.anexosField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class RelatoriosRelatorioDespesaRelatorioDespesaLancamentoAnexos
        {

            private RelatoriosRelatorioDespesaRelatorioDespesaLancamentoAnexosRelatorioDespesaLancamentoAnexo relatorioDespesaLancamentoAnexoField;

            /// <remarks/>
            public RelatoriosRelatorioDespesaRelatorioDespesaLancamentoAnexosRelatorioDespesaLancamentoAnexo RelatorioDespesaLancamentoAnexo
            {
                get
                {
                    return this.relatorioDespesaLancamentoAnexoField;
                }
                set
                {
                    this.relatorioDespesaLancamentoAnexoField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class RelatoriosRelatorioDespesaRelatorioDespesaLancamentoAnexosRelatorioDespesaLancamentoAnexo
        {

            private string urlAnexoField;

            /// <remarks/>
            public string UrlAnexo
            {
                get
                {
                    return this.urlAnexoField;
                }
                set
                {
                    this.urlAnexoField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class RelatoriosRelatorioDespesaRelatorioDespesaLancamentoContabil
        {

            private byte codigoField;

            private byte lancamentoField;

            private object contaCreditoField;

            private string contaDebitoField;

            private string codigoIntegracaoField;

            private ushort centroCustoCodigoField;

            private string centroCustoDescricaoField;

            private object projetoCodigoField;

            private object projetoDescricaoField;

            private object historicoField;

            private decimal valorField;

            private decimal valorIndiceField;

            private byte moedaCodigoField;

            private string moedaDescricaoField;

            private decimal cotacaoIndiceField;

            private object rateio01Field;

            private object rateio02Field;

            private object rateio03Field;

            private object rateio04Field;

            private object rateio05Field;

            /// <remarks/>
            public byte Codigo
            {
                get
                {
                    return this.codigoField;
                }
                set
                {
                    this.codigoField = value;
                }
            }

            /// <remarks/>
            public byte Lancamento
            {
                get
                {
                    return this.lancamentoField;
                }
                set
                {
                    this.lancamentoField = value;
                }
            }

            /// <remarks/>
            public object ContaCredito
            {
                get
                {
                    return this.contaCreditoField;
                }
                set
                {
                    this.contaCreditoField = value;
                }
            }

            /// <remarks/>
            public string ContaDebito
            {
                get
                {
                    return this.contaDebitoField;
                }
                set
                {
                    this.contaDebitoField = value;
                }
            }

            /// <remarks/>
            public string CodigoIntegracao
            {
                get
                {
                    return this.codigoIntegracaoField;
                }
                set
                {
                    this.codigoIntegracaoField = value;
                }
            }

            /// <remarks/>
            public ushort CentroCustoCodigo
            {
                get
                {
                    return this.centroCustoCodigoField;
                }
                set
                {
                    this.centroCustoCodigoField = value;
                }
            }

            /// <remarks/>
            public string CentroCustoDescricao
            {
                get
                {
                    return this.centroCustoDescricaoField;
                }
                set
                {
                    this.centroCustoDescricaoField = value;
                }
            }

            /// <remarks/>
            public object ProjetoCodigo
            {
                get
                {
                    return this.projetoCodigoField;
                }
                set
                {
                    this.projetoCodigoField = value;
                }
            }

            /// <remarks/>
            public object ProjetoDescricao
            {
                get
                {
                    return this.projetoDescricaoField;
                }
                set
                {
                    this.projetoDescricaoField = value;
                }
            }

            /// <remarks/>
            public object Historico
            {
                get
                {
                    return this.historicoField;
                }
                set
                {
                    this.historicoField = value;
                }
            }

            /// <remarks/>
            public decimal Valor
            {
                get
                {
                    return this.valorField;
                }
                set
                {
                    this.valorField = value;
                }
            }

            /// <remarks/>
            public decimal ValorIndice
            {
                get
                {
                    return this.valorIndiceField;
                }
                set
                {
                    this.valorIndiceField = value;
                }
            }

            /// <remarks/>
            public byte MoedaCodigo
            {
                get
                {
                    return this.moedaCodigoField;
                }
                set
                {
                    this.moedaCodigoField = value;
                }
            }

            /// <remarks/>
            public string MoedaDescricao
            {
                get
                {
                    return this.moedaDescricaoField;
                }
                set
                {
                    this.moedaDescricaoField = value;
                }
            }

            /// <remarks/>
            public decimal CotacaoIndice
            {
                get
                {
                    return this.cotacaoIndiceField;
                }
                set
                {
                    this.cotacaoIndiceField = value;
                }
            }

            /// <remarks/>
            public object Rateio01
            {
                get
                {
                    return this.rateio01Field;
                }
                set
                {
                    this.rateio01Field = value;
                }
            }

            /// <remarks/>
            public object Rateio02
            {
                get
                {
                    return this.rateio02Field;
                }
                set
                {
                    this.rateio02Field = value;
                }
            }

            /// <remarks/>
            public object Rateio03
            {
                get
                {
                    return this.rateio03Field;
                }
                set
                {
                    this.rateio03Field = value;
                }
            }

            /// <remarks/>
            public object Rateio04
            {
                get
                {
                    return this.rateio04Field;
                }
                set
                {
                    this.rateio04Field = value;
                }
            }

            /// <remarks/>
            public object Rateio05
            {
                get
                {
                    return this.rateio05Field;
                }
                set
                {
                    this.rateio05Field = value;
                }
            }
        }



    }
}
