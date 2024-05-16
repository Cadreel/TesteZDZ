<template>
  <v-form @submit.prevent="submit">
    <!-- Campo de seleção para o cliente -->
    <v-autocomplete v-model="clienteSelecionado" :items="clientes" item-text="nome" item-value="id" label="Cliente"
      return-object clearable required>
      <template v-slot:prepend-item>
        <v-list-item @click="criarNovoCliente">
          <v-list-item-content>
            <v-list-item-title>Criar Novo Cliente</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </template>
    </v-autocomplete>

    <!-- Campos de entrada para os dados da venda -->
    <v-select v-model="produtoSelecionado" :items="produtos" item-text="nome" item-value="id" label="Produto"
      required></v-select>
    <v-text-field v-model="quantidade" label="Quantidade" type="number" required></v-text-field>
    <v-btn @click="adicionarProduto" color="primary" v-if="!editMode">Adicionar Produto</v-btn>

    <!-- Campo para exibir os produtos comprados -->
    <v-card>
      <v-card-title>Produtos Comprados</v-card-title>
      <v-list>
        <v-list-item v-for="(produto, index) in produtosVenda" :key="index">
          <v-list-item-content>{{ produto.nome }} - {{ produto.quantidade }} unidades</v-list-item-content>
          <v-list-item-action>${{ produto.precoTotal }}</v-list-item-action>
        </v-list-item>
      </v-list>
      <v-divider></v-divider>
      <v-card-title>Total: ${{ totalVenda }}</v-card-title>
    </v-card>

    <!-- Botão para exibir os dados da venda -->
    <v-btn color="primary" @click="showData" v-if="!editMode">Exibir Dados da Venda</v-btn>

    <!-- Botão de atualizar venda -->
    <v-btn :disabled="!valid" color="success" class="mr-4" @click="atualizarVendas" v-if="editMode">
      Atualizar
    </v-btn>

    <!-- Botão de cancelar edição -->
    <v-btn color="warning" class="mr-4" @click="cancelarEdicao" v-if="editMode">
      Cancelar
    </v-btn>

    <!-- Botão de finalizar venda -->
    <v-btn :disabled="!produtosVenda.length" color="success" @click="finalizarVenda" v-if="!editMode">Finalizar
      Venda</v-btn>


    <!-- Tabela para exibir as vendas -->
    <v-simple-table>
      <template v-slot:default>
        <thead>
          <tr>
            <th class="text-left">ID</th>
            <th class="text-left">Cliente</th>
            <th class="text-left">Produto</th>
            <th class="text-left">Quantidade</th>
            <th class="text-left">Preço Unitário</th>
            <th class="text-left">Preço Total</th>
            <th class="text-left">Ações</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(venda, index) in vendas" :key="index">
            <td>{{ venda.id }}</td>
            <td>{{ clienteNome(venda.clienteId) }}</td>
            <td>{{ produtoInfo(venda.produtoId).nome }}</td>
            <td>{{ venda.quantidade }}</td>
            <td>{{ produtoInfo(venda.produtoId).preco }}</td>
            <td>{{ venda.precoTotal }}</td>
            <td>
              <v-btn icon @click="editarVenda(venda)">
                <v-icon color="blue">mdi-pencil</v-icon>
              </v-btn>
              <v-btn icon @click="excluirVenda(venda)">
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
      clienteSelecionado: null,
      produtoSelecionado: null,
      quantidade: null,
      produtosVenda: [],
      totalVenda: 0,
      clientes: [],
      produtos: [],
      vendas: [],
      snackbar: false,
      snackbarColor: 'success',
      snackbarMessage: '',
      vendaId: null
    };
  },

  methods: {
    async carregarClientes() {
      try {
        const response = await this.$axios.get('https://localhost:7226/Cliente');
        this.clientes = response.data;
      } catch (error) {
        console.error('Erro ao carregar os clientes:', error);
      }
    },
    async carregarProdutos() {
      try {
        const response = await this.$axios.get('https://localhost:7226/Produto');
        this.produtos = response.data;
      } catch (error) {
        console.error('Erro ao carregar os produtos:', error);
      }
    },
    async criarNovoCliente() {
      console.log('Criar novo cliente...');
      this.$router.push('/cliente');
    },
    async adicionarProduto() {
      const produto = this.produtos.find(p => p.id === this.produtoSelecionado);
      if (!produto) {
        this.showSnackbar('Produto inválido.', 'error');
        return;
      }

      const quantidade = parseInt(this.quantidade);
      if (isNaN(quantidade) || quantidade <= 0) {
        this.showSnackbar('Quantidade inválida.', 'error');
        return;
      }

      if (produto.quantidadeEstoque < quantidade) {
        this.showSnackbar('Estoque insuficiente.', 'error');
        return;
      }

      const precoTotal = quantidade * produto.preco;

      this.produtosVenda.push({
        id: produto.id,
        nome: produto.nome,
        quantidade: produto.quantidade,
        precoUnitario: produto.preco,
        precoTotal: precoTotal
      });

      this.totalVenda += precoTotal;

      produto.quantidadeEstoque -= quantidade;
    },
    async finalizarVenda() {
      try {
        if (!this.produtoSelecionado) {
          this.showSnackbar('Selecione um produto antes de finalizar a venda.', 'error');
          return;
        }

        const produto = this.produtos.find(p => p.id === this.produtoSelecionado);
        if (!produto) {
          this.showSnackbar('Produto inválido.', 'error');
          return;
        }

        const quantidade = parseInt(this.quantidade);
        if (isNaN(quantidade) || quantidade <= 0) {
          this.showSnackbar('Quantidade inválida.', 'error');
          return;
        }

        const precoTotal = quantidade * produto.preco;

        const venda = {
          clienteId: this.clienteSelecionado.id,
          produtoId: this.produtoSelecionado,
          quantidade: quantidade,
          precoUnitario: produto.preco,
          precoTotal: precoTotal
        };

        const response = await this.$axios.post('https://localhost:7226/Vendas', venda);
        console.log('Venda salva:', response.data);
        this.limparCampos();
        await this.showData();
        this.showSnackbar('Venda salva com sucesso!', 'success');
      } catch (error) {
        console.error('Erro ao salvar a venda:', error);
        this.showSnackbar('Erro ao salvar a venda. Por favor, tente novamente mais tarde.', 'error');
      }
    },
    async showData() {
      try {
        const response = await this.$axios.get('https://localhost:7226/Vendas');
        this.vendas = response.data;
      } catch (error) {
        console.error('Erro ao carregar vendas:', error);
      }
    },
    showSnackbar(message, color) {
      this.snackbarMessage = message;
      this.snackbarColor = color;
      this.snackbar = true;
    },
    limparCampos() {
      this.clienteSelecionado = null;
      this.produtoSelecionado = null;
      this.quantidade = null;
    },
    async editarVenda(venda) {
      this.editMode = true;
      this.clienteSelecionado = this.clientes.find(cli => cli.id === venda.clienteId);
      this.produtoSelecionado = this.produtos.find(prod => prod.id === venda.produtoId);
      this.quantidade = venda.quantidade;
      this.vendaId = venda.id;

      // Calculando o preço total com base na quantidade atualizada
      const precoTotal = venda.quantidade * this.produtoSelecionado.preco;

      // Atualizando o preço total do produto na lista de produtos vendidos
      this.produtosVenda = [{
        id: this.produtoSelecionado.id,
        nome: this.produtoSelecionado.nome,
        quantidade: venda.quantidade,
        precoUnitario: this.produtoSelecionado.preco,
        precoTotal: precoTotal
      }];

      // Calculando o novo total da venda
      this.totalVenda = precoTotal;
    },

    async atualizarVendas() {
      try {
        // Encontrar o novo produto selecionado
        const novoProduto = this.produtos.find(prod => prod.id === this.produtoSelecionado);

        // Calcular o novo preço total com base na quantidade e no preço unitário do novo produto
        const novoPrecoTotal = this.quantidade * novoProduto.preco;

        // Construir o objeto da venda atualizada
        const vendaAtualizada = {
          id: this.vendaId,
          clienteId: this.clienteSelecionado.id,
          produtoId: this.produtoSelecionado,
          quantidade: this.quantidade,
          precoUnitario: novoProduto.preco,
          precoTotal: novoPrecoTotal
        };

        // Enviar a solicitação PUT para atualizar a venda no servidor
        const response = await this.$axios.put(`https://localhost:7226/Vendas/${this.vendaId}`, vendaAtualizada);

        // Limpar os campos e atualizar a interface
        this.limparCampos();
        this.editMode = false;
        await this.showData();
        this.showSnackbar('Venda atualizada com sucesso!', 'success');
      } catch (error) {
        // Lidar com erros de forma adequada
        console.error('Erro ao atualizar a venda:', error);
        this.showSnackbar('Erro ao atualizar a venda. Por favor, tente novamente mais tarde.', 'error');
      }
    },





    cancelarEdicao() {
      this.editMode = false;
      this.limparCampos();
    },
    async excluirVenda(venda) {
      try {
        await this.$axios.delete(`https://localhost:7226/Vendas/${venda.id}`);
        await this.showData();
        this.showSnackbar('Produto excluído com sucesso!', 'success');
      } catch (error) {
        console.error('Erro ao excluir a venda:', error);
        this.showSnackbar('Erro ao excluir a venda. Por favor, tente novamente mais tarde.', 'error');
      }
      console.log('Excluir venda:', venda);
    },
    produtoNome(produtoId) {
      const produto = this.produtos.find(prod => prod.id === produtoId);
      return produto ? produto.nome : '';
    },
    clienteNome(clienteId) {
      const cliente = this.clientes.find(cli => cli.id === clienteId);
      return cliente ? cliente.nome : '';
    },
    produtoInfo(produtoId) {
      const produto = this.produtos.find(prod => prod.id === produtoId);
      return produto ? { nome: produto.nome, preco: produto.preco } : { nome: '', preco: 0 };
    },
  },
  created() {
    this.carregarClientes();
    this.carregarProdutos();
    this.showData();
  }
};
</script>

<style scoped>
.v-footer {
  margin-top: 20px;
}
</style>
