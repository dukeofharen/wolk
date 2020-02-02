/// <reference types="Cypress" />

describe('Login test', () => {
    beforeEach(() => {
        cy.visit('/');
    });
    
    it('Logging in with correct credentials should redirect to overview page', () => {
        cy.get('input[name="email"]').type('wolk@example.com');
        cy.get('input[name="password"]').type('Password123!@');
        cy.get('#login').click();
        cy.url().should('include', '/overview');
    });

    it('Logging in with incorrect credentials should not redirect and show error', () => {
        cy.get('input[name="email"]').type('wolk@example.com');
        cy.get('input[name="password"]').type('WRONGPASS');
        cy.get('#login').click();
        cy.url().should('include', '/login');
        cy.get('.v-snack__content').should('contain', 'The provided credentials are incorrect.');
    });
});