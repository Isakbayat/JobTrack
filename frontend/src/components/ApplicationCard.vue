<script setup>
const props = defineProps({
  application: {
    type: Object,
    required: true,
  },
})

const emit = defineEmits(['edit', 'delete'])

function formatDate(date) {
  const datePart = date.split('T')[0]
  const [year, month, day] = datePart.split('-')

  return `${day}/${month}/${year}`
}

function getStatusClass(applicationStatus) {
  return `status-${applicationStatus.toLowerCase()}`
}
</script>

<template>
  <article class="application-card">
    <div class="card-header">
      <div>
        <p class="company-name">
          {{ props.application.company }}
        </p>

        <h3>
          {{ props.application.position }}
        </h3>
      </div>

      <span
        class="status-badge"
        :class="getStatusClass(props.application.status)"
      >
        {{ props.application.status }}
      </span>
    </div>

    <div class="application-meta">
      <span>Applied</span>

      <strong>
        {{ formatDate(props.application.appliedDate) }}
      </strong>
    </div>

    <div class="card-actions">
      <button
        class="edit-button"
        type="button"
        @click="emit('edit', props.application)"
      >
        Edit
      </button>

      <button
        class="delete-button"
        type="button"
        @click="emit('delete', props.application)"
      >
        Delete
      </button>
    </div>
  </article>
</template>

<style scoped>
.application-card {
  padding: 24px;
  border: 1px solid #1e293b;
  border-radius: 16px;
  background: #0f172a;
  transition:
    transform 0.2s ease,
    border-color 0.2s ease;
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

button {
  border: 0;
  border-radius: 10px;
  cursor: pointer;
  font-weight: 700;
  transition:
    transform 0.15s ease,
    background 0.15s ease;
}

button:hover {
  transform: translateY(-1px);
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

@media (max-width: 700px) {
  .card-header {
    flex-direction: column;
  }
}
</style>