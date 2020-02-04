/// <reference types="Cypress" />

describe('Notenook test', () => {
    beforeEach(() => {
        cy.login();
    });

    it('Create new notebook', () => {
        const notebookName = new Date().getTime().toString();
        cy.get('#add-notebook').click();
        cy.get('input[name="name"]').type(notebookName);
        cy.get('#save-notebook').click();
    });
});