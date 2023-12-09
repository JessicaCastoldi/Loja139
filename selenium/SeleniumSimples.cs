// 1 - Namespace ~ Pacote ~ Grupo de Classes ~ Workspace
namespace SeleniumSimples;

// 2-  Bibliotecas ~ Dependências

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V115.FedCm;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

// 3- Classe 
[TestFixture]  // Configura como uma classe de teste

public class AdicionarProdutoNoCarrinhoTest{
    //3.1 Atributos  ~ Características ~ Campos

    private IWebDriver driver;  //objeto do Selenium Webdriver

    //3.2  Função ou Método de Apoio
    //Função de leitura do arquivo CSV - massa de teste
    public static IEnumerable<TestCaseData> lerDadosDeTeste(){
         using (var reader = new StreamReader(@"/home/jessica/Documents/Iterasys/Loja139/data/login.csv"))  //declaramos um objeto chamado reader que lê o conteúdo do CSV
         {
             //Pular a linha do cabeçalho do CSV
             reader.ReadLine();

             while (!reader.EndOfStream)  //Faça enquanto não for o final do arquivo  --While  --! (não) reader.EndOfStream
             {
                var linha = reader.ReadLine();  //Ler a linha correspondente - cortar a fileira do chocolate
                var valores = linha.Split(", "); 

                yield return new TestCaseData(valores[0], valores [1], valores [2]);
             } //fim do while - funciona como uma mola
         };
    }

    //3.3 Configurações de Antes do Teste
   [SetUp] //Configura um método para ser executado antes dos testes
    public void Before(){
        new DriverManager().SetUpDriver(new ChromeConfig()); //Faz o Download e instalação da versão mais recente do ChromeDriver
        driver = new ChromeDriver();  //Instancia o objeto do Selenium com o Chrome
        driver.Manage().Window.Maximize(); //Maximiza a janela do navegador
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(50000); // Configura uma espera implicita de 5 segundos para qualquer elemento aparecer

  
    }//Fim do Before

    // 3.4 Configurações de Depois do Teste
    [TearDown] // Configura um método para ser usado depois dos testes
    public void After(){
        driver.Quit(); //Destruir o objeto do Selenium na mémoria

    } //Fim do After

    //3.5 O(s) Teste (s)
    [Test] //Indica que é um método de teste
    public void Login(){
        //Abrir o navegador e acessar o site
        driver.Navigate().GoToUrl("https://www.saucedemo.com");
        //Thread.Sleep(5000); //Pausa Forçada - Remover antes de publicar

        //Preencher o Usuário
        driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
        //Preencher a Senha
        driver.FindElement(By.Name("password")).SendKeys("secret_sauce");
        //Clicar no botão Login
        driver.FindElement(By.CssSelector("input.submit-button.btn_action")).Click();
        //verificar se fizemos o login no sistema, confirmando um texto ancora.
        Assert.AreEqual( driver.FindElement(By.CssSelector("span.title")).Text, "Products");

        Thread.Sleep(2000);
    } //Fim do Login

   
    [Test, TestCaseSource("lerDadosDeTeste")] 
    public void LoginPositivoDDT(String username, String password, String resultadoEsperado){
        driver.Navigate().GoToUrl("https://www.saucedemo.com");
        driver.FindElement(By.Id("user-name")).SendKeys(username);
        driver.FindElement(By.Name("password")).SendKeys(password);
        driver.FindElement(By.CssSelector("input.submit-button.btn_action")).Click();
        Assert.That( resultadoEsperado, Is.EqualTo(driver.FindElement(By.CssSelector("span.title")).Text));
        Thread.Sleep(2000);
    } //Fim do LoginPositivoDDT



} //Fim da classe