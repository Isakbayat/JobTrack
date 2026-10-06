<script setup>
  import { onMounted, ref } from 'vue'

  const applications = ref([])
  const loading = ref(true)
  const error = ref('')

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

  onMounted(() => {
    loadApplications()
  })
</script>

<template>
  <main class="app">
    <h1>JobTrack</h1>
    <p>Track your job and LIA applications in one place.</p>

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
