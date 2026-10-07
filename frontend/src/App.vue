<script setup>
  import { onMounted, ref } from 'vue'

  const applications = ref([])
  const loading = ref(true)
  const error = ref('')
  const saving = ref(false)
  const editingId = ref(null)

  const company = ref('')
  const position = ref('')
  const appliedDate = ref('')
  const status = ref('Applied')

  async function loadApplications() {
    try {
      const response = await fetch('/api/Applications')

      if (!response.ok) {
        throw new Error('Could not load applications')
      }

      applications.value = await response.json()
    } catch (err) {
      error.value = err.message
    } finally {
      loading.value = false
    }
  }

  function resetForm() {
    company.value = ''
    position.value = ''
    appliedDate.value = ''
    status.value = 'Applied'
    editingId.value = null
  }

  function startEdit(application) {
    editingId.value = application.id
    company.value = application.company
    position.value = application.position
    appliedDate.value = application.appliedDate.split('T')[0]
    status.value = application.status
  }

  function cancelEdit() {
    resetForm()
  }

  async function saveApplication() {
    saving.value = true
    error.value = ''

    const applicationData = {
      company: company.value,
      position: position.value,
      appliedDate: appliedDate.value,
      status: status.value,
    }

    try {
      let response

      if (editingId.value !== null) {
        response = await fetch(`/api/Applications/${editingId.value}`, {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(applicationData),
        })
      } else {
        response = await fetch('/api/Applications', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(applicationData),
        })
      }

      if (!response.ok) {
        throw new Error(
          editingId.value !== null
            ? 'Could not update application'
            : 'Could not create application',
        )
      }

      resetForm()
      await loadApplications()
    } catch (err) {
      error.value = err.message
    } finally {
      saving.value = false
    }
  }

  async function deleteApplication(application) {
    const confirmed = window.confirm(
      `Are you sure you want to delete the application for ${application.company}?`,
    )

    if (!confirmed) {
      return
    }

    error.value = ''

    try {
      const response = await fetch(`/api/Applications/${application.id}`, {
        method: 'DELETE',
      })

      if (!response.ok) {
        throw new Error('Could not delete application')
      }

      if (editingId.value === application.id) {
        resetForm()
      }

      await loadApplications()
    } catch (err) {
      error.value = err.message
    }
  }

  onMounted(() => {
    loadApplications()
  })
</script>

<template>
  <main class="app">
    <h1>JobTrack</h1>
    <p>Track your job and LIA applications in one place.</p>

    <section class="create-application">
      <h2>
        {{ editingId !== null ? 'Edit application' : 'Add application' }}
      </h2>

      <form @submit.prevent="saveApplication">
        <label>
          Company
          <input v-model="company" type="text" required />
        </label>

        <label>
          Position
          <input v-model="position" type="text" required />
        </label>

        <label>
          Applied date
          <input v-model="appliedDate" type="date" required />
        </label>

        <label>
          Status
          <select v-model="status">
            <option value="Applied">Applied</option>
            <option value="Interview">Interview</option>
            <option value="Rejected">Rejected</option>
            <option value="Offer">Offer</option>
          </select>
        </label>

        <button type="submit" :disabled="saving">
          {{
            saving
              ? 'Saving...'
              : editingId !== null
                ? 'Save changes'
                : 'Add application'
          }}
        </button>

        <button v-if="editingId !== null"
                type="button"
                class="cancel-button"
                @click="cancelEdit">
          Cancel
        </button>
      </form>
    </section>

    <section class="applications">
      <h2>Applications</h2>

      <p v-if="loading">Loading applications...</p>

      <p v-else-if="error">{{ error }}</p>

      <div v-else
           v-for="application in applications"
           :key="application.id"
           class="application-card">
        <h3>{{ application.company }}</h3>
        <p>{{ application.position }}</p>
        <span>{{ application.status }}</span>

        <div class="card-actions">
          <button type="button" @click="startEdit(application)">
            Edit
          </button>

          <button type="button"
                  class="delete-button"
                  @click="deleteApplication(application)">
            Delete
          </button>
        </div>
      </div>
    </section>
  </main>
</template>

<style scoped>
  .app {
    max-width: 900px;
    margin: 0 auto;
    padding: 40px 20px;
  }

  h1 {
    font-size: 40px;
    margin-bottom: 8px;
  }

  .create-application {
    margin-top: 40px;
  }

  form {
    display: grid;
    gap: 16px;
    max-width: 500px;
  }

  label {
    display: grid;
    gap: 6px;
  }

  input,
  select {
    padding: 10px;
    border: 1px solid #555;
    border-radius: 6px;
    font: inherit;
  }

  button {
    padding: 12px 18px;
    border: none;
    border-radius: 6px;
    cursor: pointer;
    font: inherit;
  }

    button:disabled {
      cursor: not-allowed;
      opacity: 0.6;
    }

  .cancel-button {
    margin-top: -6px;
  }

  .applications {
    margin-top: 40px;
  }

  .application-card {
    padding: 20px;
    margin-top: 16px;
    border: 1px solid #444;
    border-radius: 10px;
  }

    .application-card h3 {
      margin: 0 0 8px;
    }

    .application-card p {
      margin: 0 0 10px;
    }

  .card-actions {
    margin-top: 16px;
  }

    .card-actions button {
      padding: 8px 14px;
    }

  .delete-button {
    margin-left: 8px;
  }
</style>
