<script setup>
</script>

<template>
  <main class="app">
    <h1>JobTrack</h1>
    <p>Track your job and LIA applications in one place.</p>

    <section class="applications">
      <h2>Applications</h2>

      <div class="application-card">
        <h3>Telenor</h3>
        <p>System Developer Intern</p>
        <span>Applied</span>
      </div>

      <div class="application-card">
        <h3>SEB</h3>
        <p>.NET Developer Intern</p>
        <span>Interview</span>
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
