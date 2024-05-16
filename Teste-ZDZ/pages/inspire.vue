<template>
  <div class="job-description-container">
    <h2>Descrição da Vaga</h2>
    <p>
      Você é um iniciante no mundo da programação e quer trabalhar em projetos desafiadores e inovadores? Então esta vaga é para você! Buscamos um candidato que saiba conceituar e executar código limpo e eficiente para desenvolver software de alta qualidade. Você será responsável por testar seu código, corrigir erros e melhorar o desempenho do software. Além disso, você terá a oportunidade de apoiar nossos clientes e parceiros, resolvendo suas dúvidas e problemas de software.
    </p>

    <h3>Responsabilidades:</h3>
    <ul>
      <li>Diagnosticar e resolver problemas de software</li>
      <li>Escrever código legível e robusto para software e aplicativos, seguindo as boas práticas de desenvolvimento</li>
      <li>Desenvolver, implementar e testar APIs</li>
      <li>Contribuir com ideias e sugestões para os projetos de desenvolvimento de software</li>
    </ul>

    <h3>Qualificações:</h3>
    <ul>
      <li>Conhecimento básico de linguagens de programação e bancos de dados relacionais</li>
      <li>Habilidades de depuração e solução de problemas</li>
    </ul>

    <h3>Tecnologias que utilizamos:</h3>
    <ul>
      <li>C#</li>
      <li>Javascript</li>
      <li>Typescript</li>
      <li>SQL</li>
      <li>HTML</li>
      <li>CSS</li>
      <li>VueJs</li>
      <li>Vuetify</li>
    </ul>

    <p>
      Prezado(a) candidato(a),
      Tem um teste para ser executado, como critério de seleção. Segue vídeo explicativo para ajudar: <a href="https://www.youtube.com/watch?v=dTBAsoeCs4E">Assista ao vídeo explicativo</a>.
      <br>
      Descrição do teste:
      O teste consiste em desenvolver uma solução para um problema que envolva controle de dados, como por exemplo, controle financeiro, controle de estoque, histórico de manutenção, agendamentos ou outro tema de sua preferência. Para isso, você deverá seguir os passos abaixo:
    </p>
    <ol>
      <li>Modelar os dados que serão utilizados na solução, usando diagramas ou tabelas.</li>
      <li>Esboçar as telas que irão compor a interface da solução, usando desenhos ou ferramentas de prototipação.</li>
      <li>Implementar as telas usando VueJS + framework Nuxt + Vuetify (<a href="https://nuxtjs.org">nuxtjs.org</a>), seguindo o esboço feito no passo anterior.</li>
      <li>Criar um projeto C# de API com o Visual Studio 2022 Community, que será responsável por fornecer e receber os dados das telas.</li>
      <li>Criar endpoints na API para cadastrar, consultar, atualizar e excluir os dados da solução, conforme as necessidades das telas.</li>
    </ol>
    <p>
      Não é necessário que a solução seja um produto de software completo, mas sim que demonstre suas habilidades e conhecimentos nas tecnologias solicitadas. Você pode entrar em contato conosco para tirar dúvidas e pedir sugestões, estamos à disposição.
    </p>
  </div>
</template>

<script>
export default {
  name: 'JobDescription'
}
</script>

<style scoped>
.job-description-container {
  max-width: 800px;
  margin: 50px auto;
  font-family: 'Arial', sans-serif;
  color: #ffffff;
}

.job-description-container h2 {
  font-size: 32px;
  margin-bottom: 20px;
  color: #007bff; /* Azul */
}

.job-description-container h3 {
  font-size: 24px;
  margin-top: 30px;
  color: #ffffff; /* Cinza escuro */
}

.job-description-container p,
.job-description-container ul {
  font-size: 18px;
  line-height: 1.6;
}

.job-description-container a {
  color: #007bff;
  text-decoration: none;
}

.job-description-container ol {
  margin-left: 20px;
}

.job-description-container li {
  margin-bottom: 10px;
}
</style>
