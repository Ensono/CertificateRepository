Feature: Wrapper should provide access to underlying certificate store
    Certificate should be accessible through wrapper

  @CleanupCertificatesOnError
  Scenario: Read certificate from the certificate store by subject name
    Given the certificate 'sampleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a store wrapper
	  And I open the 'My' store for the 'CurrentUser' with the 'ReadOnly' flag
	  And I call find certificate by subject name 'Sample Certificate'
    Then there should be '1' certificates in the collection
	  And one certificate retrieved should match the thumbprint 'EFFF7FD55F1F37B67CCE6F539071AD13A4BFA92C'
	Then any certificates should be cleaned up

 @CleanupCertificatesOnError
 Scenario: Read certificate from the certificate store by thumbprint
    Given the certificate 'sampleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a store wrapper
	  And I open the 'My' store for the 'CurrentUser' with the 'ReadOnly' flag
	  And I call find certificate by thumbprint 'EFFF7FD55F1F37B67CCE6F539071AD13A4BFA92C'
    Then there should be '1' certificates in the collection
	  And one certificate retrieved should match the thumbprint 'EFFF7FD55F1F37B67CCE6F539071AD13A4BFA92C'
	Then any certificates should be cleaned up
 
 @CleanupCertificatesOnError
 Scenario: Read certificate from the certificate store by subject distinguished name
    Given the certificate 'sampleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a store wrapper
	  And I open the 'My' store for the 'CurrentUser' with the 'ReadOnly' flag
	  And I call find certificate by subject distinguished name 'CN=Sample Certificate'
    Then there should be '1' certificates in the collection
	  And one certificate retrieved should match the thumbprint 'EFFF7FD55F1F37B67CCE6F539071AD13A4BFA92C'
	Then any certificates should be cleaned up