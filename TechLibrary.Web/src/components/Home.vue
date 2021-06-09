<template>
    <div class="home">
        <h1>{{ msg }}</h1>

        <!-- Search/paging -->
        <b-row>
            <!-- Search bar -->
            <b-col xs="12" md="6">
                <b-input-group>
                    <b-form-input
                        v-model="text"
                        placeholder="Search Text" />
                    <b-input-group-append>
                        <b-button
                            v-if="text"
                            variant="secondary"
                            @click="resetSearch"
                        >
                            Clear
                        </b-button>
                        <b-button
                            variant="primary"
                            @click="manualSearch"
                        >
                            Search
                        </b-button>
                    </b-input-group-append>
                </b-input-group>
            </b-col>

            <!-- Paging -->
            <b-col xs="12" md="6">
                <b-pagination
                    v-model="page"
                    :total-rows="totalItems"
                    :per-page="itemsPerPage"
                    align="right"
                    aria-controls="books-table" />
            </b-col>
        </b-row>

        <!-- Books table -->
        <b-table
            id="books-table"
            striped
            hover
            :items="items"
            :fields="fields"
            responsive="sm"
            empty-text="No books found">
            <template v-slot:cell(thumbnailUrl)="data">
                <b-img :src="data.value" thumbnail fluid></b-img>
            </template>
            <template v-slot:cell(title_link)="data">
                <b-link :to="{ name: 'book_view', params: { 'id' : data.item.bookId } }">{{ data.item.title }}</b-link>
            </template>
        </b-table>
    </div>
</template>

<script>
    import axios from 'axios';

    export default {
        name: 'Home',
        props: {
            msg: String
        },
        created() {
            this.searchBooks()
        },
        data: () => ({
            fields: [
                { key: 'thumbnailUrl', label: 'Book Image' },
                { key: 'title_link', label: 'Book Title', sortable: true, sortDirection: 'desc' },
                { key: 'isbn', label: 'ISBN', sortable: true, sortDirection: 'desc' },
                { key: 'descr', label: 'Description', sortable: true, sortDirection: 'desc' }
            ],
            text: '',
            items: [],
            page: 1,
            itemsPerPage: 10,
            totalPages: 0,
            totalItems: 0
        }),
        methods: {
            resetPaging() {
                this.page = 1
                this.totalPages = 0
                this.totalItems = 0
            },
            async searchBooks() {
                const { text, page, itemsPerPage } = this

                const resp = await axios.post(
                    "https://localhost:5001/books/search",
                    {
                        text,
                        page,
                        itemsPerPage
                    })

                // Update values
                const { items, totalItems, totalPages } = resp.data
                Object.assign(this, { items, totalItems, totalPages })
            },
            async manualSearch() {
                // Reset paging values
                this.resetPaging()

                await this.searchBooks()
            },
            async resetSearch() {
                // Reset paging values
                this.resetPaging()

                // Clear items and text
                this.text = ''
                this.items = []

                await this.searchBooks()
            }
        },
        watch: {
            page() {
                this.searchBooks()
            }
        }
    };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>

