import {DueStatusType} from "@/models/enums/dueStatusType";
<template>
    <div>
        <v-row no-gutters class="filter">
            <v-col cols="12">
                <v-row>
                    <v-select
                            :items="projectTags"
                            placeholder="Filter on project tag..."
                            v-model="projectTag"
                            @change="filterChanged"
                            clearable
                    />
                </v-row>
            </v-col>
            <v-col cols="12">
                <v-row>
                    <v-select
                            :items="contextTags"
                            placeholder="Filter on context tag..."
                            v-model="contextTag"
                            @change="filterChanged"
                            clearable
                    />
                </v-row>
            </v-col>
            <v-col cols="12">
                <v-row>
                    <v-select
                            :items="dueStatusTypes"
                            placeholder="Filter on due status..."
                            v-model="dueStatus"
                            @change="filterChanged"
                            item-text="value"
                            item-value="key"
                            clearable
                    />
                </v-row>
            </v-col>
            <v-col cols="12">
                <v-row>
                    <v-switch
                            v-model="excludeDone"
                            @change="filterChanged"
                            label="Exclude finished todo items" />
                </v-row>
            </v-col>
        </v-row>
    </div>
</template>

<script lang="ts">
    import {Component, Prop, Vue} from "vue-property-decorator";
    import {TodoTxtModel} from "@/models/todoTxtModel";
    import {extractContextTags, extractProjectTags} from "@/services/todoTxtService";
    import {TodoTxtFilterModel} from "@/models/store/todoTxtFilterModel";
    import {KeyValuePair} from "@/models/keyValuePair";
    import {DueStatusType, getDueStatusTypeArray} from "@/models/enums/dueStatusType";

    @Component({
        components: {  }
    })
    export default class TodoTxtFilter extends Vue {
        projectTag: string = "";
        contextTag: string = "";
        excludeDone: boolean = false;
        dueStatus: DueStatusType = DueStatusType.NOT_SET;
        dueStatusTypes: Array<KeyValuePair<DueStatusType, string>> = getDueStatusTypeArray();
        
        @Prop()
        models!: TodoTxtModel[];
        
        created() {
            let filter = this.$store.getters.todoTxtFilter;
            this.projectTag = filter.projectTagFilter;
            this.contextTag = filter.contextTagFilter;
            this.excludeDone = filter.excludeDone;
            this.dueStatus = filter.dueStatus;
        }
        
        filterChanged() {
            let filterModel: TodoTxtFilterModel = {
                projectTagFilter: this.projectTag,
                contextTagFilter: this.contextTag,
                excludeDone: this.excludeDone,
                dueStatus: this.dueStatus
            };
            this.$store.commit("SET_TODOTXT_FILTER", filterModel);
        }
        
        get projectTags() {
            return extractProjectTags(this.models);
        }

        get contextTags() {
            return extractContextTags(this.models);
        }
    }
</script>

<style scoped>
    .filter {
        margin-left: 10px;
        margin-right: 10px;
    }
</style>