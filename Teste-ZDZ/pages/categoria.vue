<template>
  <v-form @submit.prevent="submit">
    <!-- Campos de entrada para nome e descrição da categoria -->
    <v-text-field v-model="categoria.nome" :min="5" :rules="nomeRules" label="Categoria" required></v-text-field>
    <v-text-field v-model="categoria.descricao" label="Descrição"></v-text-field>

    <!-- Botões de salvar e mostrar dados -->
    <v-btn :disabled="!valid" color="success" class="mr-4" type="submit" v-if="!editMode">
      Salvar
    </v-btn>
    <v-btn :disabled="!valid" color="success" class="mr-4" @click="atualizarCategoria" v-if="editMode">
      Atualizar
    </v-btn>
    <v-btn color="warning" class="mr-4" @click="cancelarEdicao" v-if="editMode">
      Cancelar
    </v-btn>
    <v-btn color="success" class="mr-4" @click="showData" v-if="!editMode">
      Mostrar Dados
    </v-btn>

    <!-- Tabela para exibir as categorias -->
    <v-simple-table>
      <template v-slot:default>
        <thead>
          <tr>
            <th class="text-left">ID</th>
            <th class="text-left">Nome</th>
            <th class="text-left">Descrição</th>
            <th class="text-left">Ações</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(cat, index) in categorias" :key="index">
            <td>{{ cat.id }}</td>
            <td>{{ cat.nome }}</td>
            <td>{{ cat.descricao }}</td>
            <td>
              <v-btn icon @click="editarCategoria(cat)">
                <v-icon color="blue">mdi-pencil</v-icon>
              </v-btn>
              <v-btn icon @click="excluirCategoria(cat)">
                <v-icon color="red">mdi-delete</v-icon>
              </v-btn>
            </td>
          </tr>
        </tbody>
      </template>
    </v-simple-table>
    
    <!-- Rodapé -->
    <v-footer class="mt-12"></v-footer>

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
      categoria: {
        id: null,
        nome: '',
        descricao: ''
      },
      nomeRules: [
        v => !!v || 'Nome é obrigatório',
        v => (v && v.length > 5) || 'Nome precisa ter pelo menos 5 caracteres',
      ],
      categorias: [],
      snackbar: false,
      snackbarColor: 'success',
      snackbarMessage: ''
    };
  },
  methods: {
    async submit() {
      try {
        const { id, ...categoriaSemId } = this.categoria;
        const response = await this.$axios.post('https://localhost:7226/Categoria', categoriaSemId);
        this.categoria = { nome: '', descricao: '' };
        await this.showData();
        this.showSnackbar('Categoria salva com sucesso!', 'success');
      } catch (error) {
        console.error('Erro ao salvar categoria:', error);
        this.showSnackbar('Erro ao salvar categoria. Por favor, tente novamente mais tarde.', 'error');
      }
    },
    async editarCategoria(categoria) {
      this.editMode = true;
      this.categoria.id = categoria.id;
      this.categoria.nome = categoria.nome;
      this.categoria.descricao = categoria.descricao;
    },
    cancelarEdicao() {
      this.editMode = false;
      // Limpar os campos ou restaurar os valores originais, se necessário
      this.categoria = { id: null, nome: '', descricao: '' };
    },
    async atualizarCategoria() {
      try {
        await this.$axios.put(`https://localhost:7226/Categoria/${this.categoria.id}`, this.categoria);
        this.editMode = false;
        this.categoria = { id: null, nome: '', descricao: '' };
        await this.showData();
        this.showSnackbar('Categoria atualizada com sucesso!', 'success');
      } catch (error) {
        console.error('Erro ao atualizar categoria:', error);
        this.showSnackbar('Erro ao atualizar categoria. Por favor, tente novamente mais tarde.', 'error');
      }
    },
    async excluirCategoria(categoria) {
      try {
        await this.$axios.delete(`https://localhost:7226/Categoria/${categoria.id}`);
        this.categorias = this.categorias.filter(cat => cat.id !== categoria.id);
        this.showSnackbar('Categoria excluída com sucesso!', 'success');
      } catch (error) {
        console.error('Erro ao excluir categoria:', error);
        this.showSnackbar('Erro ao excluir categoria. Por favor, tente novamente mais tarde.', 'error');
      }
    },
    async showData(){
      try {
        const responseGet = await this.$axios.get('https://localhost:7226/Categoria');
        this.categorias = responseGet.data;
      } catch (error) {
        console.error('Erro ao obter categorias:', error);
        this.showSnackbar('Erro ao obter categorias. Por favor, tente novamente mais tarde.', 'error');
      }
    },
    showSnackbar(message, color) {
      this.snackbarMessage = message;
      this.snackbarColor = color;
      this.snackbar = true;
    }
  }
};
</script>

<style scoped>
.v-footer {
  margin-top: 20px;
}
</style>
