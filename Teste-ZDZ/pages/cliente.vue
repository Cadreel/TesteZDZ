<template>
  <v-form @submit.prevent="submit">
      <!-- Campos de entrada para os dados do cliente -->
      <v-text-field v-model="cliente.nome" label="Nome do Cliente" required></v-text-field>
      <v-text-field v-model="cliente.email" label="Email do Cliente" required></v-text-field>
      <v-text-field v-model="cliente.telefone" label="Telefone do Cliente" required></v-text-field>
      <v-text-field v-model="cliente.endereco" label="Endereço do Cliente" required></v-text-field>

      <!-- Botões de salvar e mostrar dados -->
      <v-btn :disabled="!valid || !camposPreenchidos()" color="success" class="mr-4" type="submit" v-if="!editMode">
          Salvar
      </v-btn>

      <!-- Botão de atualizar cliente -->
      <v-btn :disabled="!valid || !camposPreenchidos()" color="success" class="mr-4" @click="atualizarCliente" v-if="editMode">
          Atualizar
      </v-btn>

      <!-- Botão para cancelar edição -->
      <v-btn color="warning" class="mr-4" @click="cancelarEdicao" v-if="editMode">
          Cancelar
      </v-btn>

      <!-- Botão para exibir os dados dos clientes  -->
      <v-btn color="success" class="mr-4" @click="showData" v-if="!editMode">
          Mostrar Clientes
      </v-btn>

      <!-- Tabela para exibir os clientes -->
      <v-simple-table>
          <template v-slot:default>
              <thead>
                  <tr>
                      <th class="text-left">ID</th>
                      <th class="text-left">Nome</th>
                      <th class="text-left">Email</th>
                      <th class="text-left">Telefone</th>
                      <th class="text-left">Endereço</th>
                      <th class="text-left">Ações</th>
                  </tr>
              </thead>
              <tbody>
                  <tr v-for="(cliente, index) in clientes" :key="index">
                      <td>{{ cliente.id }}</td>
                      <td>{{ cliente.nome }}</td>
                      <td>{{ cliente.email }}</td>
                      <td>{{ cliente.telefone }}</td>
                      <td>{{ cliente.endereco }}</td>
                      <td>
                          <v-btn icon @click="editarCliente(cliente)">
                              <v-icon color="blue">mdi-pencil</v-icon>
                          </v-btn>
                          <v-btn icon @click="excluirCliente(cliente)">
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
          cliente: {
              nome: '',
              email: '',
              telefone: '',
              endereco: ''
          },
          clientes: [],
          snackbar: false,
          snackbarColor: 'success',
          snackbarMessage: ''
      };
  },
  methods: {
      async submit() {
          // Verifica se todos os campos obrigatórios estão preenchidos
          if (!this.camposPreenchidos()) {
              this.showSnackbar('Por favor, preencha todos os campos obrigatórios.', 'error');
              return;
          }

          try {
              const { id, ...clienteSemId } = this.cliente;
              const response = await this.$axios.post('https://localhost:7226/Cliente', clienteSemId);
              console.log('Cliente salvo:', response.data);
              this.limparCampos();
              await this.showData();
              this.showSnackbar('Cliente salvo com sucesso!', 'success');
          } catch (error) {
              console.error('Erro ao salvar o cliente:', error);
              this.showSnackbar('Erro ao salvar o cliente. Por favor, tente novamente mais tarde.', 'error');
          }
      },
      async showData() {
          try {
              const response = await this.$axios.get('https://localhost:7226/Cliente');
              this.clientes = response.data;
          } catch (error) {
              console.error('Erro ao carregar clientes:', error);
          }
      },
      async editarCliente(cliente) {
          this.editMode = true;
          this.cliente = { ...cliente };
      },
      async atualizarCliente() {
          try {
              const response = await this.$axios.put(`https://localhost:7226/Cliente/${this.cliente.id}`, this.cliente);
              console.log('Cliente atualizado:', response.data);
              this.limparCampos();
              this.editMode = false;
              await this.showData();
              this.showSnackbar('Cliente atualizado com sucesso!', 'success');
          } catch (error) {
              console.error('Erro ao atualizar o cliente:', error);
              this.showSnackbar('Erro ao atualizar o cliente. Por favor, tente novamente mais tarde.', 'error');
          }
      },
      async excluirCliente(cliente) {
          try {
              await this.$axios.delete(`https://localhost:7226/Cliente/${cliente.id}`);
              console.log('Cliente excluído:', cliente);
              await this.showData();
              this.showSnackbar('Cliente excluído com sucesso!', 'success');
          } catch (error) {
              console.error('Erro ao excluir o cliente:', error);
              this.showSnackbar('Erro ao excluir o cliente. Por favor, tente novamente mais tarde.', 'error');
          }
      },
      cancelarEdicao() {
          this.editMode = false;
          this.limparCampos();
      },
      limparCampos() {
          this.cliente = {
              nome: '',
              email: '',
              telefone: '',
              endereco: ''
          };
      },
      showSnackbar(message, color) {
          this.snackbarMessage = message;
          this.snackbarColor = color;
          this.snackbar = true;
      },
      camposPreenchidos() {
          // Verifica se todos os campos obrigatórios estão preenchidos
          return (
              this.cliente.nome.trim() !== ''
          );
      }
  },
  async created() {
      await this.showData();
  }
};
</script>

<style scoped>
.v-footer {
  margin-top: 20px;
}
</style>

