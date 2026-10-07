<script setup>
  import { onMounted, ref } from 'vue'

  const applications = ref([])
  const loading = ref(true)
  const error = ref('')
  const message = ref('')
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

    message.value = ''

    window.scrollTo({
      top: 0,
      behavior: 'smooth',
    })
  }

  function cancelEdit() {
    resetForm()
    message.value = ''
  }

  async function saveApplication() {
    saving.value = true
    error.value = ''
    message.value = ''

    const isEditing = editingId.value !== null

    const applicationData = {
      company: company.value,
      position: position.value,
      appliedDate: appliedDate.value,
      status: status.value,
    }

    try {
      let response

      if (isEditing) {
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
          isEditing
            ? 'Could not update application'
            : 'Could not create application',
        )
      }

      resetForm()
      await loadApplications()

      message.value = isEditing
        ? 'Application updated successfully.'
        : 'Application added successfully.'
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
    message.value = ''

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
      message.value = 'Application deleted successfully.'
    } catch (err) {
      error.value = err.message
    }
  }

  function formatDate(date) {
    const datePart = date.split('T')[0]
    const [year, month, day] = datePart.split('-')

    return `${day}/${month}/${year}`
  }

  function getStatusClass(applicationStatus) {
    return `status-${applicationStatus.toLowerCase()}`
  }

  onMounted(() => {
    loadApplications()
  })
</script>

<template>
  <div class="page-shell">
    <main class="app">
      <header class="hero">
        <div>
          <p class="eyebrow">APPLICATION TRACKER</p>
          <h1>JobTrack</h1>
          <p class="subtitle">
            Keep track of your job and LIA applications in one place.
          </p>
        </div>

        <div class="application-count">
          <strong>{{ applications.length }}</strong>
          <span>
            {{ applications.length === 1 ? 'application' : 'applications' }}
          </span>
        </div>
      </header>

      <p v-if="message" class="message success-message">
        {{ message }}
      </p>

      <p v-if="error" class="message error-message">
        {{ error }}
      </p>

      <section class="panel form-panel">
        <div class="section-heading">
          <div>
            <p class="section-label">
              {{ editingId !== null ? 'UPDATE' : 'NEW APPLICATION' }}
            </p>

            <h2>
              {{ editingId !== null ? 'Edit application' : 'Add application' }}
            </h2>
          </div>
        </div>

        <form @submit.prevent="saveApplication">
          <div class="form-grid">
            <label>
              <span>Company</span>
              <input v-model="company"
                     type="text"
                     placeholder="e.g. Telenor"
                     required />
            </label>

            <label>
              <span>Position</span>
              <input v-model="position"
                     type="text"
                     placeholder="e.g. System Developer Intern"
                     required />
            </label>

            <label>
              <span>Applied date</span>
              <input v-model="appliedDate" type="date" required />
            </label>

            <label>
              <span>Status</span>
              <select v-model="status">
                <option value="Applied">Applied</option>
                <option value="Interview">Interview</option>
                <option value="Offer">Offer</option>
                <option value="Rejected">Rejected</option>
              </select>
            </label>
          </div>

          <div class="form-actions">
            <button class="primary-button" type="submit" :disabled="saving">
              {{
                saving
                  ? 'Saving...'
                  : editingId !== null
                    ? 'Save changes'
                    : 'Add application'
              }}
            </button>

            <button v-if="editingId !== null"
                    class="secondary-button"
                    type="button"
                    @click="cancelEdit">
              Cancel
            </button>
          </div>
        </form>
      </section>

      <section class="applications-section">
        <div class="section-heading applications-heading">
          <div>
            <p class="section-label">OVERVIEW</p>
            <h2>Applications</h2>
          </div>
        </div>

        <div v-if="loading" class="panel state-panel">
          Loading applications...
        </div>

        <div v-else-if="applications.length === 0"
             class="panel state-panel empty-state">
          <h3>No applications yet</h3>
          <p>Add your first application using the form above.</p>
        </div>

        <div v-else class="applications-grid">
          <article v-for="application in applications"
                   :key="application.id"
                   class="application-card">
            <div class="card-header">
              <div>
                <p class="company-name">{{ application.company }}</p>
                <h3>{{ application.position }}</h3>
              </div>

              <span class="status-badge"
                    :class="getStatusClass(application.status)">
                {{ application.status }}
              </span>
            </div>

            <div class="application-meta">
              <span>Applied</span>
              <strong>{{ formatDate(application.appliedDate) }}</strong>
            </div>

            <div class="card-actions">
              <button class="edit-button"
                      type="button"
                      @click="startEdit(application)">
                Edit
              </button>

              <button class="delete-button"
                      type="button"
                      @click="deleteApplication(application)">
                Delete
              </button>
            </div>
          </article>
        </div>
      </section>
    </main>
  </div>
</template>

<style scoped>
  :global(*) {
    box-sizing: border-box;
  }

  :global(body) {
    margin: 0;
    min-width: 320px;
    min-height: 100vh;
    background: #0b1120;
    color: #e5e7eb;
    font-family: Inter, ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif;
  }

  :global(button),
  :global(input),
  :global(select) {
    font: inherit;
  }

  .page-shell {
    min-height: 100vh;
    background: radial-gradient(circle at top left, rgba(37, 99, 235, 0.16), transparent 34%), #0b1120;
  }

  .app {
    width: min(1050px, calc(100% - 40px));
    margin: 0 auto;
    padding: 64px 0 80px;
  }

  .hero {
    display: flex;
    align-items: flex-end;
    justify-content: space-between;
    gap: 30px;
    margin-bottom: 42px;
  }

  .eyebrow,
  .section-label {
    margin: 0 0 8px;
    color: #60a5fa;
    font-size: 12px;
    font-weight: 800;
    letter-spacing: 0.16em;
  }

  h1 {
    margin: 0;
    color: #ffffff;
    font-size: clamp(42px, 7vw, 68px);
    line-height: 1;
    letter-spacing: -0.04em;
  }

  .subtitle {
    max-width: 560px;
    margin: 16px 0 0;
    color: #94a3b8;
    font-size: 18px;
    line-height: 1.6;
  }

  .application-count {
    display: flex;
    min-width: 145px;
    flex-direction: column;
    padding: 20px 24px;
    border: 1px solid #1e293b;
    border-radius: 16px;
    background: rgba(15, 23, 42, 0.75);
    text-align: center;
  }

    .application-count strong {
      color: #ffffff;
      font-size: 30px;
    }

    .application-count span {
      margin-top: 2px;
      color: #94a3b8;
      font-size: 13px;
    }

  .panel {
    border: 1px solid #1e293b;
    border-radius: 18px;
    background: rgba(15, 23, 42, 0.82);
    box-shadow: 0 20px 50px rgba(0, 0, 0, 0.16);
  }

  .form-panel {
    padding: 30px;
  }

  .section-heading h2 {
    margin: 0;
    color: #f8fafc;
    font-size: 26px;
  }

  .form-grid {
    display: grid;
    grid-template-columns: repeat(2, minmax(0, 1fr));
    gap: 20px;
    margin-top: 26px;
  }

  label {
    display: grid;
    gap: 8px;
  }

    label span {
      color: #cbd5e1;
      font-size: 14px;
      font-weight: 600;
    }

  input,
  select {
    width: 100%;
    padding: 12px 14px;
    border: 1px solid #334155;
    border-radius: 10px;
    outline: none;
    background: #111827;
    color: #f8fafc;
    transition: border-color 0.2s ease, box-shadow 0.2s ease;
  }

    input::placeholder {
      color: #64748b;
    }

    input:focus,
    select:focus {
      border-color: #3b82f6;
      box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.15);
    }

  .form-actions {
    display: flex;
    gap: 12px;
    margin-top: 24px;
  }

  button {
    border: 0;
    border-radius: 10px;
    cursor: pointer;
    font-weight: 700;
    transition: transform 0.15s ease, opacity 0.15s ease, background 0.15s ease;
  }

    button:hover {
      transform: translateY(-1px);
    }

    button:disabled {
      cursor: not-allowed;
      opacity: 0.6;
      transform: none;
    }

  .primary-button {
    padding: 12px 20px;
    background: #2563eb;
    color: #ffffff;
  }

    .primary-button:hover {
      background: #1d4ed8;
    }

  .secondary-button {
    padding: 12px 20px;
    background: #334155;
    color: #f8fafc;
  }

  .applications-section {
    margin-top: 50px;
  }

  .applications-heading {
    margin-bottom: 20px;
  }

  .applications-grid {
    display: grid;
    gap: 16px;
  }

  .application-card {
    padding: 24px;
    border: 1px solid #1e293b;
    border-radius: 16px;
    background: #0f172a;
    transition: transform 0.2s ease, border-color 0.2s ease;
  }

    .application-card:hover {
      transform: translateY(-2px);
      border-color: #334155;
    }

  .card-header {
    display: flex;
    align-items: flex-start;
    justify-content: space-between;
    gap: 20px;
  }

  .company-name {
    margin: 0 0 5px;
    color: #60a5fa;
    font-size: 14px;
    font-weight: 800;
    text-transform: uppercase;
    letter-spacing: 0.08em;
  }

  .application-card h3 {
    margin: 0;
    color: #f8fafc;
    font-size: 21px;
  }

  .status-badge {
    display: inline-flex;
    align-items: center;
    flex-shrink: 0;
    padding: 6px 11px;
    border-radius: 999px;
    font-size: 12px;
    font-weight: 800;
  }

  .status-applied {
    background: rgba(59, 130, 246, 0.15);
    color: #93c5fd;
  }

  .status-interview {
    background: rgba(245, 158, 11, 0.15);
    color: #fcd34d;
  }

  .status-offer {
    background: rgba(34, 197, 94, 0.15);
    color: #86efac;
  }

  .status-rejected {
    background: rgba(239, 68, 68, 0.15);
    color: #fca5a5;
  }

  .application-meta {
    display: flex;
    gap: 8px;
    margin-top: 18px;
    color: #64748b;
    font-size: 14px;
  }

    .application-meta strong {
      color: #cbd5e1;
      font-weight: 600;
    }

  .card-actions {
    display: flex;
    gap: 10px;
    margin-top: 22px;
    padding-top: 18px;
    border-top: 1px solid #1e293b;
  }

  .edit-button,
  .delete-button {
    padding: 9px 15px;
  }

  .edit-button {
    background: #1e293b;
    color: #e2e8f0;
  }

    .edit-button:hover {
      background: #334155;
    }

  .delete-button {
    background: rgba(239, 68, 68, 0.12);
    color: #fca5a5;
  }

    .delete-button:hover {
      background: rgba(239, 68, 68, 0.2);
    }

  .state-panel {
    padding: 32px;
    color: #94a3b8;
    text-align: center;
  }

  .empty-state h3 {
    margin: 0;
    color: #f8fafc;
  }

  .empty-state p {
    margin: 8px 0 0;
  }

  .message {
    margin: 0 0 20px;
    padding: 12px 16px;
    border-radius: 10px;
    font-size: 14px;
    font-weight: 600;
  }

  .success-message {
    border: 1px solid rgba(34, 197, 94, 0.28);
    background: rgba(34, 197, 94, 0.1);
    color: #86efac;
  }

  .error-message {
    border: 1px solid rgba(239, 68, 68, 0.28);
    background: rgba(239, 68, 68, 0.1);
    color: #fca5a5;
  }

  @media (max-width: 700px) {
    .app {
      width: min(100% - 28px, 1050px);
      padding-top: 38px;
    }

    .hero {
      align-items: stretch;
      flex-direction: column;
    }

    .application-count {
      width: fit-content;
    }

    .form-grid {
      grid-template-columns: 1fr;
    }

    .form-panel {
      padding: 22px;
    }

    .card-header {
      flex-direction: column;
    }
  }
</style>
