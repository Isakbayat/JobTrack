<script setup>
  import { onMounted, ref } from 'vue'

  const applications = ref([])
  const loading = ref(true)
  const error = ref('')
  const saving = ref(false)

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

  async function createApplication() {
    saving.value = true
    error.value = ''

    const newApplication = {
      company: company.value,
      position: position.value,
      appliedDate: appliedDate.value,
      status: status.value,
    }

    try {
      const response = await fetch('/api/Applications', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newApplication),
      })

      if (!response.ok) {
        throw new Error('Could not create application')
      }

      company.value = ''
      position.value = ''
      appliedDate.value = ''
      status.value = 'Applied'

      await loadApplications()
    } catch (err) {
      error.value = err.message
    } finally {
      saving.value = false
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
      <h2>Add application</h2>

      <form @submit.prevent="createApplication">
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
          {{ saving ? 'Saving...' : 'Add application' }}
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
</style>
