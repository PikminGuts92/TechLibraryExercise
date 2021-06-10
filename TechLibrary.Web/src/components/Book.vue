<template>
    <div v-if="book">
        <b-card :title="book.title"
                :img-src="book.thumbnailUrl"
                img-alt="Image"
                img-top
                tag="article"
                style="max-width: 30rem;"
                class="mb-2">
            
            <!-- Read-only book description -->
            <b-card-text v-if="!editEnabled">
                {{ book.descr }}
            </b-card-text>

            <b-form-textarea
                class="mb-3"
                v-else
                v-model="book.descr" />

            <!-- Back button -->
            <b-button
                class="mr-2"
                to="/"
                variant="primary"
                :disabled="editEnabled">
                Back
            </b-button>

            <!-- Edit button -->
            <b-button
                v-if="!editEnabled"
                variant="outline-secondary"
                @click="enableEdit">
                Edit
            </b-button>

            <!-- Cancel button -->
            <b-button
                v-if="editEnabled"
                class="mr-2"
                variant="danger"
                @click="cancelEdit">
                Cancel
            </b-button>

            <!-- Save button -->
            <b-button
                v-if="editEnabled"
                variant="success"
                @click="saveEdit">
                Save
            </b-button>
        </b-card>
    </div>
</template>

<script>
    import axios from 'axios';

    export default {
        name: 'Book',
        props: ["id"],
        data: () => ({
            book: null,
            oldBook: null,
            editEnabled: false
        }),
        mounted() {
            axios.get(`https://localhost:5001/books/${this.id}`)
                .then(response => {
                    this.book = response.data;
                });
        },
        methods: {
            enableEdit() {
                this.oldBook = { ...this.book } // Shallow copy
                this.editEnabled = true
            },
            cancelEdit() {
                // Revert changes
                this.book = this.oldBook
                this.editEnabled = false
            },
            async saveEdit() {
                await this.updateServer()
                this.editEnabled = false
            },
            async updateServer() {
                await axios.post(
                    `https://localhost:5001/books/${this.id}`, this.book)
                    .then(response => {
                        this.book = response.data;
                    })
            }
        }
    }
</script>