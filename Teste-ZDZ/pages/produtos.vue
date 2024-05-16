<template>
  <v-form @submit.prevent="submit">
    <!-- Campos de entrada para os dados do produto -->
    <v-text-field v-model="produto.nome" label="Nome do Produto" required></v-text-field>
    <v-select v-model="produto.categoriaId" :items="categorias" item-value="id" item-text="nome" label="Categoria" required></v-select>
    <v-text-field v-model="produto.unidade" label="Unidade"></v-text-field>
    <v-text-field v-model="produto.preco" label="Preço (R$)" type="number" step="0.01" min="0" required></v-text-field>
    <v-text-field v-model="produto.quantidadeEstoque" label="Quantidade em Estoque" type="number" required></v-text-field>

    <!-- Botões de salvar e mostrar dados -->
    <v-btn :disabled="!valid" color="success" class="mr-4" type="submit" v-if="!editMode">
      Salvar
    </v-btn>

    <!-- Botão de atualizar produto -->
    <v-btn :disabled="!valid" color="success" class="mr-4" @click="atualizarProduto" v-if="editMode">
      Atualizar
    </v-btn>

    <!-- Botão para cancelar edição -->
    <v-btn color="warning" class="mr-4" @click="cancelarEdicao" v-if="editMode">
      Cancelar
    </v-btn>

    <!-- Botão para exibir os dados dos produtos -->
    <v-btn color="success" class="mr-4" @click="showData" v-if="!editMode">
      Mostrar Produtos
    </v-btn>

    <!-- Tabela para exibir os produtos -->
    <v-simple-table>
      <template v-slot:default>
        <thead>
          <tr>
            <th class="text-left">ID</th>
            <th class="text-left">Nome</th>
            <th class="text-left">Categoria</th>
            <th class="text-left">Unidade</th>
            <th class="text-left">Preço (R$)</th>
            <th class="text-left">Quantidade em Estoque</th>
            <th class="text-left">Ações</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(produto, index) in produtos" :key="index">
            <td>{{ produto.id }}</td>
            <td>{{ produto.nome }}</td>
            <td>{{ categoriaNome(produto.categoriaId) }}</td>
            <td>{{ produto.unidade }}</td>
            <td>{{ formatarPreco(produto.preco) }}</td>
            <td>{{ produto.quantidadeEstoque }}</td>
            <td>
              <v-btn icon @click="editarProduto(produto)">
                <v-icon color="blue">mdi-pencil</v-icon>
              </v-btn>
              <v-btn icon @click="excluirProduto(produto)">
                <v-icon color="red">mdi-delete</v-icon>
              </v-btn>
            </td>
          </tr>
        </tbody>
      </template>
    </v-simple-table>
    
    <!-- Mensagem de feedback -->
    <v-snackbar v-model="snackbar" :color="snackbarColor" top>
      {{ snackbarMessage }}
      <template v-slot:action="{ attrs }">
        <v-btn color="white" text v-bind="attrs" @click="snackbar = false">Fechar</v-btn>
      </template>
    </v-snackbar>
  </v-form>
</template>

<script>
export default {
  data() {
    return {
      valid: true,
      editMode: false,
      produto: {
        id: null,
        nome: '',
        categoriaId: null,
        unidade: '',
        preco: null,
        quantidadeEstoque: null
      },
      categorias: [],
      produtos: [],
      snackbar: false,
      snackbarColor: 'success',
      snackbarMessage: ''
    };
  },
  methods: {
    async submit() {
      try {
        const { id, ...produtoSemId } = this.produto;
        const response = await this.$axios.post('https://localhost:7226/Produto', produtoSemId);
        console.log('Produto salvo:', response.data);
        this.limparCampos();
        await this.showData();
        this.showSnackbar('Produto salvo com sucesso!', 'success');
      } catch (error) {
        console.error('Erro ao salvar o produto:', error);
        this.showSnackbar('Erro ao salvar o produto. Por favor, tente novamente mais tarde.', 'error');
      }
    },
    async showData() {
      try {
        const response = await this.$axios.get('https://localhost:7226/Produto');
        this.produtos = response.data;
      } catch (error) {
        console.error('Erro ao carregar produtos:', error);
      }
    },
    async editarProduto(produto) {
      this.editMode = true;
      this.produto.id = produto.id;
      this.produto.nome = produto.nome;
      this.produto.categoriaId = produto.categoriaId;
      this.produto.unidade = produto.unidade;
      this.produto.preco = produto.preco;
      this.produto.quantidadeEstoque = produto.quantidadeEstoque;
    },
    async atualizarProduto() {
      try {
        const response = await this.$axios.put(`https://localhost:7226/Produto/${this.produto.id}`, this.produto);
        console.log('Produto atualizado:', response.data);
        this.limparCampos();
        this.editMode = false;
        await this.showData();
        this.showSnackbar('Produto atualizado com sucesso!', 'success');
      } catch (error) {
        console.error('Erro ao atualizar o produto:', error);
        this.showSnackbar('Erro ao atualizar o produto. Por favor, tente novamente mais tarde.', 'error');
      }
    },
    async excluirProduto(produto) {
      try {
        await this.$axios.delete(`https://localhost:7226/Produto/${produto.id}`);
        console.log('Produto excluído:', produto);
        await this.showData();
        this.showSnackbar('Produto excluído com sucesso!', 'success');
      } catch (error) {
        console.error('Erro ao excluir o produto:', error);
        this.showSnackbar('Erro ao excluir o produto. Por favor, tente novamente mais tarde.', 'error');
      }
    },
    cancelarEdicao() {
      this.editMode = false;
      this.limparCampos();
    },
    async carregarCategorias() {
      try {
        const response = await this.$axios.get('https://localhost:7226/Categoria');
        this.categorias = response.data;
      } catch (error) {
        console.error('Erro ao carregar categorias:', error);
      }
    },
    limparCampos() {
      this.produto.id = null;
      this.produto.nome = '';
      this.produto.categoriaId = null;
      this.produto.unidade = '';
      this.produto.preco = null;
      this.produto.quantidadeEstoque = null;
    },
    showSnackbar(message, color) {
      this.snackbarMessage = message;
      this.snackbarColor = color;
      this.snackbar = true;
    },
    categoriaNome(categoriaId) {
      const categoria = this.categorias.find(cat => cat.id === categoriaId);
      return categoria ? categoria.nome : '';
    },
    formatarPreco(preco) {
      if (preco !== null && preco !== undefined) {
        return preco.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
      }
      return '';
    }
  },
  created() {
    this.carregarCategorias();
    this.showData();
  }
};
</script>

<style scoped>
.v-footer {
  margin-top: 20px;
}
</style>

