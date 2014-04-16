Feature: CertificateRepository exposes X509 Certificate Store
	Must be able to access certificates by Subject Name
	Must be able to access multiple certificates by Subject Name
	Must be able to access certificates by Thumbprint

  @CleanupCertificatesOnError
  Scenario: Access single certificate from My store by Subject Name 
    Given the certificate 'singleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a certificate repository
	  And set the Store Name to 'My'
	  And set the Store Location to 'CurrentUser'
	  And I call FindBySubjectName with subject 'Single Certificate'
    Then there should only be '1' certificate in the collection
	  And one certificate retrieved should match the thumbprint 'B0B7EEEA3EB2F7A927EB1B075BA6EF9906D928AE' 
	Then any certificates should be cleaned up

  @CleanupCertificatesOnError
  Scenario: Access two certificates from My store by Subject Name 
    Given the certificate 'sampleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	  And the certificate 'additionalCertificate.pfx' has been loaded using password 'abc123'	
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a certificate repository
	  And set the Store Name to 'My'
	  And set the Store Location to 'CurrentUser'
	  And I call FindBySubjectName with subject 'Sample Certificate'
    Then there should be '2' certificates in the collection
	  And one certificate retrieved should match the thumbprint 'C21B70C4DA42C98C8C696A782016585B5BF5223D'
	  And one certificate retrieved should match the thumbprint 'EFFF7FD55F1F37B67CCE6F539071AD13A4BFA92C'
	Then any certificates should be cleaned up

  @CleanupCertificatesOnError
  Scenario: Find two similarlly named certificates from My store by Subject Name 
    Given the certificate 'rpstsCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	  And the certificate 'rpCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a certificate repository
	  And set the Store Name to 'My'
	  And set the Store Location to 'CurrentUser'
	  And I call FindBySubjectName with subject 'RP'
    Then there should be '2' or more certificates in the collection
	  And one certificate retrieved should match the thumbprint '0BF2B563528296FB66D4AD94523648281D419078'
	  And one certificate retrieved should match the thumbprint '66EF82D22C40F9A2576117C6BB69971348694AA1'
	Then any certificates should be cleaned up

  @CleanupCertificatesOnError
  Scenario: Read certificate from the certificate store by thumbprint
    Given the certificate 'sampleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a certificate repository
	  And set the Store Name to 'My'
	  And set the Store Location to 'CurrentUser'
	  And I call FindByThumbprint with thumbprint 'EFFF7FD55F1F37B67CCE6F539071AD13A4BFA92C'
	Then one certificate retrieved should match the thumbprint 'EFFF7FD55F1F37B67CCE6F539071AD13A4BFA92C' 
	Then any certificates should be cleaned up

  @CleanupCertificatesOnError
  Scenario: Access two out of three certificates from My store by Subject Name 
    Given the certificate 'sampleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	  And the certificate 'additionalCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	  And the certificate 'singleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a certificate repository
	  And set the Store Name to 'My'
	  And set the Store Location to 'CurrentUser'
	  And I call FindBySubjectName with subject 'Sample Certificate'
    Then there should be '2' certificates in the collection
	  And one certificate retrieved should match the thumbprint 'C21B70C4DA42C98C8C696A782016585B5BF5223D'
	  And one certificate retrieved should match the thumbprint 'EFFF7FD55F1F37B67CCE6F539071AD13A4BFA92C'
	Then any certificates should be cleaned up

  @CleanupCertificatesOnError
  Scenario: Access single certificate from My store by Subject Distinguished Name 
    Given the certificate 'singleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a certificate repository
	  And set the Store Name to 'My'
	  And set the Store Location to 'CurrentUser'
	  And I call FindBySubjectDistinguishedName with subject 'CN=Single Certificate'
    Then there should only be '1' certificate in the collection
	  And one certificate retrieved should match the thumbprint 'B0B7EEEA3EB2F7A927EB1B075BA6EF9906D928AE' 
	Then any certificates should be cleaned up

  @CleanupCertificatesOnError
  Scenario: Access two certificates from My store by Subject Distinguished Name 
    Given the certificate 'sampleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	  And the certificate 'additionalCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a certificate repository
	  And set the Store Name to 'My'
	  And set the Store Location to 'CurrentUser'
	  And I call FindBySubjectDistinguishedName with subject 'CN=Sample Certificate'
    Then there should be '2' certificates in the collection
	  And one certificate retrieved should match the thumbprint 'C21B70C4DA42C98C8C696A782016585B5BF5223D'
	  And one certificate retrieved should match the thumbprint 'EFFF7FD55F1F37B67CCE6F539071AD13A4BFA92C'
	Then any certificates should be cleaned up

  @CleanupCertificatesOnError
  Scenario: Access two out of three certificates from My store by Subject Distinguished Name 
    Given the certificate 'sampleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	  And the certificate 'additionalCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	  And the certificate 'singleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a certificate repository
	  And set the Store Name to 'My'
	  And set the Store Location to 'CurrentUser'
	  And I call FindBySubjectDistinguishedName with subject 'CN=Sample Certificate'
    Then there should be '2' certificates in the collection
	  And one certificate retrieved should match the thumbprint 'C21B70C4DA42C98C8C696A782016585B5BF5223D'
	  And one certificate retrieved should match the thumbprint 'EFFF7FD55F1F37B67CCE6F539071AD13A4BFA92C'
	Then any certificates should be cleaned up