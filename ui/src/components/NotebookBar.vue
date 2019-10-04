<template>
  <div>{{notebooks}}</div>
</template>
<script>
import { mapState } from "vuex";
export default {
    mounted() {
        this.$store.dispatch('loadNotebooks');
    },
    computed: mapState([
        'notebooks'
    ])
};
</script>

<style scoped>
</style>